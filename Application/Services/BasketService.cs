using Domain.BackendResponses;
using Domain.Entities.Basket;
using Domain.Entities.Items.ItemTrading;
using Domain.Interfaces.DbContexts;
using Domain.Interfaces.Services.BasketService;
using Domain.Interfaces.Services.BasketService.Dto;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class BasketService : IBasketService
    {
        private readonly ICatalogDbContext _catalogContext;
        private readonly IUserDbContext _userDbContext;

        public BasketService(ICatalogDbContext catalogContext, IUserDbContext userDbContext)
        {
            _catalogContext = catalogContext;
            _userDbContext = userDbContext;
        }

        /// <summary>
        /// Add item to user basket
        /// </summary>
        /// <param name="userId">Owner id</param>
        /// <param name="item">Item to be added</param>
        /// <param name="quantity">Quantity to be added</param>
        /// <returns></returns>
        public async Task<MethodResult<BasketResponse>> AddItemAsync(Guid userId, Guid itemId, int quantity = 1)
        {
            var basket = await GetBasketInternalAsync(userId);

            var item = await _catalogContext.ItemsTrading.FirstOrDefaultAsync(x => x.Id == itemId);

            if (item is null)
            {
                var errorBasket = await CollectBasketAsync(basket);
                return new MethodResult<BasketResponse>(errorBasket, ["Товар не найден"], Domain.CoreEnums.Enums.MethodResults.Conflict);
            }

            if (item.SellingType == Domain.CoreEnums.Enums.SellingTypes.Auction)
            {
                var errorBasket = await CollectBasketAsync(basket);
                return new MethodResult<BasketResponse>(errorBasket, ["Аукционный товар не может быть добавлен в корзину"], Domain.CoreEnums.Enums.MethodResults.Conflict);
            }

            //delete item if status is not Published
            if (item.Status != Domain.CoreEnums.Enums.ItemTradingStatus.Published)
            {
                basket.DeleteItem(item.Id);
            }

            else
            {
                basket.AddItem(item.Id, quantity);
            }

            var basketDto = await CollectBasketAsync(basket);

            await _catalogContext.SaveChangesAsync();

            return new (basketDto, [], Domain.CoreEnums.Enums.MethodResults.Ok);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">Owner id</param>
        /// <param name="item">Item to be removed</param>
        /// <param name="quantity">Quantity to be removed</param>
        /// <returns></returns></param>
        /// <returns></returns>
        public async Task<BasketResponse> RemoveItemAsync(Guid userId, Guid itemId, int quantity = 1)
        {
            var basket = await GetBasketInternalAsync(userId);

            var item = await _catalogContext.ItemsTrading.FirstOrDefaultAsync(x => x.Id == itemId);

            if (item is null)
            {
                basket.DeleteItem(itemId);
            }

            else
            {
                //delete item if status is not Published
                if (item.Status != Domain.CoreEnums.Enums.ItemTradingStatus.Published)
                {
                    basket.DeleteItem(item.Id);
                }

                else
                {
                    basket.RemoveItem(item.Id, quantity);
                }
            }

            var basketDto = await CollectBasketAsync(basket);

            await _catalogContext.SaveChangesAsync();

            return basketDto;
        }

        /// <summary>
        /// Get user basket. If basket does not exist, it creates
        /// </summary>
        /// <param name="userId">Owner id</param>
        /// <returns></returns>
        private async Task<Basket> GetBasketInternalAsync(Guid userId)
        {
            var basket = await _catalogContext.Baskets
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (basket is null)
            {
                Basket newBasket = new(userId);
                _catalogContext.Baskets.Add(newBasket);
                await _catalogContext.SaveChangesAsync();
                return newBasket;
            }

            return basket;
        }

        /// <summary>
        /// Get user basket. If basket does not exist, it creates
        /// </summary>
        /// <param name="userId">Owner id</param>
        /// <returns></returns>
        public async Task<BasketResponse> GetBasketAsync(Guid userId)
        {
            var basket = await _catalogContext.Baskets
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (basket is null)
            {
                basket = new(userId);
                _catalogContext.Baskets.Add(basket);
            }

            var basketDto = await CollectBasketAsync(basket);

            await _catalogContext.SaveChangesAsync();

            return basketDto;
        }

        private async Task<BasketResponse> CollectBasketAsync(Basket basket)
        {
            List<ItemTrading> itemsInBasket = [];

            foreach (var item in basket.Items)
            {
                var itemTrading = await _catalogContext.ItemsTrading
                    .Where(x => x.Id == item.ItemId && x.Status == Domain.CoreEnums.Enums.ItemTradingStatus.Published)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (itemTrading is null)
                {
                    basket.DeleteItem(item.ItemId);
                }

                else
                {
                    itemsInBasket.Add(itemTrading);
                }
            }

            var sellersGroups = await CollectSellersGroupsAsync(itemsInBasket, basket);

            float totalPrice = 0;

            foreach (var seller in sellersGroups)
            {
                totalPrice += seller.TotalPrice;
            }

            return new(sellersGroups, totalPrice);
        }

        private async Task<IEnumerable<BasketSellerGroup>> CollectSellersGroupsAsync(IEnumerable<ItemTrading> items, Basket basket)
        {
            List<BasketSellerGroup> sellerGroups = [];

            var itemsTradingSellersGroups = items.GroupBy(x => x.UserId);

            foreach (var sellerGroup in itemsTradingSellersGroups)
            {
                var seller = await _userDbContext.Users
                    .Where(x => x.Id == sellerGroup.Key)
                    .Select(x => new { x.Id, x.UserName, x.IsUserAsShopOption, x.FirstName, x.SecondName, x.ShopTitle, x.UserLogoSmallImage, x.ShopLogoSmallImage })
                    .FirstOrDefaultAsync();

                //user is deleted, so remove this items from basket
                if (seller is null)
                {
                    foreach (var item in sellerGroup)
                    {
                        basket.DeleteItem(item.Id);
                    }

                    continue;
                }

                string sellerName = seller.IsUserAsShopOption ? seller.ShopTitle : seller.UserName!;

                BasketSellerGroup newSellerGroup = new(sellerName, seller.UserName!);

                sellerGroups.Add(newSellerGroup);

                var termsGroups = await CollectTermsGroupsAsync(sellerGroup.ToList(), basket);

                float totalPrice = 0;

                foreach (var termsGroup in termsGroups)
                {
                    totalPrice += termsGroup.TotalPrice;
                }

                newSellerGroup.TotalPrice = totalPrice;

                newSellerGroup.BasketSendingTermsGroups = termsGroups;
            }

            return sellerGroups;
        }

        private async Task<IEnumerable<BasketSendingTermsGroup>> CollectTermsGroupsAsync(IEnumerable<ItemTrading> items, Basket basket)
        {
            List<BasketSendingTermsGroup> basketSendingTermsGroups = [];

            var itemsTermsGroups = items.GroupBy(x => x.DaysForShipment);

            foreach (var itemsGroup in itemsTermsGroups)
            {
                BasketSendingTermsGroup basketSendingTermsGroup = new(itemsGroup.Key);

                basketSendingTermsGroups.Add(basketSendingTermsGroup);

                var responseItems = await CollectItemsAsync(itemsGroup.ToList(), basket);

                float totalPrice = 0;

                foreach (var item in responseItems)
                {
                    totalPrice += item.ItemsTotalPrice;
                }

                basketSendingTermsGroup.TotalPrice = totalPrice;

                basketSendingTermsGroup.BasketItems = responseItems;
            }

            return basketSendingTermsGroups;
        }

        private async Task<IEnumerable<BasketItemResponse>> CollectItemsAsync(IEnumerable<ItemTrading> items, Basket basket)
        {
            List<BasketItemResponse> itemsDto = [];

            foreach (var item in items)
            {
                var itemImage = await _catalogContext.ItemsTradingsImages
                    .Where(x => x.OwnerEntity.Id == item.Id)
                    .AsNoTracking()
                    .Select(x => x.SmallImagePath)
                    .FirstOrDefaultAsync();

                var quantity = basket.Items.First(x => x.ItemId == item.Id).Quantity;
                var totalItemPrice = item.MaxPrice * (float)quantity;

                BasketItemResponse basketItemResponse = new(item.Id, item.Title, itemImage ?? string.Empty, quantity, item.MaxPrice, totalItemPrice);

                itemsDto.Add(basketItemResponse);
            }

            return itemsDto;
        }
    }
}
