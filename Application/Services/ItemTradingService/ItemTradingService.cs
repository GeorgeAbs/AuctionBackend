using Domain.BackendResponses;
using Domain.Common;
using Domain.Common.Dto;
using Domain.Entities.Addresses;
using Domain.Entities.AuctionSlots;
using Domain.Entities.Catalog.CatalogProperty;
using Domain.Entities.Images;
using Domain.Entities.Items.ItemTrading;
using Domain.Interfaces.DbContexts;
using Domain.Interfaces.Services;
using Domain.Interfaces.Services.ItemService.ItemTradingService;
using Domain.Interfaces.Services.ItemService.ItemTradingService.Dto;
using Domain.Interfaces.Services.ItemService.ItemTradingService.Dto.AuctionItem;
using Domain.Interfaces.Services.ItemService.ItemTradingService.Dto.PropertiesForGetItemRequest;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.Services.ItemTradingService
{
    public class ItemTradingService<T> : IItemTradingService<T> where T : ItemTrading
    {
        private readonly IAutoModerator _autoModerator;
        private readonly ICatalogDbContext _catalogContext;
        private readonly IUserDbContext _userContext;
        private readonly IDistributedCache _cache;
        private readonly IImageService _imageService;

        public ItemTradingService(IAutoModerator autoModerator, ICatalogDbContext catalogContext, IUserDbContext userContext, IDistributedCache cache, IImageService imageService)
        {
            _autoModerator = autoModerator;
            _catalogContext = catalogContext;
            _userContext = userContext;
            _cache = cache;
            _imageService = imageService;
        }

        public async Task<MethodResult<ItemTradingGetResponse>> GetAsync(Guid itemId)
        {
            var item = await _catalogContext.ItemsTrading
                .Where(x => x.Id == itemId)
                .Select(x => new
                    {
                        x.Id,
                        x.Title,
                        x.Description,
                        images = x.Images.Select(i => i.SmallImagePath),
                        x.SellingType,
                        x.AuctionEndingTime,
                        x.FreeQuantity,
                        x.MinPrice,
                        cateroryName = x.CatalogCategory.Name,
                        intProps = x.IntProperties.Select(p => new IntItemPropertyGetItemResponse(p.PropertyValue, p.PropertyName.Name)),
                        floatProps = x.FloatProperties.Select(p => new FloatItemPropertyGetItemResponse(p.PropertyValue, p.PropertyName.Name)),
                        stringProps = x.StringProperties.Select(p => new StringItemPropertyGetItemResponse(p.PropertyValue, p.PropertyName.Name)),
                        boolProps = x.BoolProperties.Select(p => new BoolItemPropertyGetItemResponse(p.PropertyValue, p.PropertyName.Name)),
                        x.DaysForShipment,
                        x.PaymentMethods,
                        x.UserId,
                        questionsAnswers = x.Questions.Where(q => q.Answer != null).Select(q => new QuestionAnswerItemGetResponse(q.CreationTime, q.Text, q.Answer!.Text))
                    }
                )
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (item == null)
            {
                return new MethodResult<ItemTradingGetResponse>(null, [], Domain.CoreEnums.Enums.MethodResults.NotFound);
            }

            #region itemInfo
            var itemInfo = new ItemTradingGetItemInfoResponse(item.Id,
                item.Title,
                item.Description,
                item.images,
                item.SellingType,
                item.AuctionEndingTime,
                item.FreeQuantity,
                item.MinPrice,
                item.cateroryName,
                item.intProps,
                item.floatProps,
                item.stringProps,
                item.boolProps);

            itemInfo.SetQuestionsAnswers(item.questionsAnswers);

            var reviews = await _userContext.ItemTradingReviews
                .Where(r => r.ItemId == itemId && r.ReviewResponse != null)
                .Select(r => new ReviewResponseItemGetResponse(r.CreationTime, r.ItemTitle, r.Text, r.Mark, r.ReviewImages.Select(i => i.SmallImagePath), r.ReviewResponse!.Text))
                .ToListAsync();

            itemInfo.SetReviewsResponses(reviews);

            if (itemInfo.SellingType == Domain.CoreEnums.Enums.SellingTypes.Auction)
            {
                var slotsInfo = await _catalogContext.ItemTradingAuctionSlots
                .Where(slot => slot.ItemId == itemId)
                .Select(slot => new ItemTradingAuctionSlotInfoGetItemResponse(
                    slot.Id,
                    slot.Title,
                    slot.AuctionSlotNum,
                    slot.Description,
                    (int)slot.Price,
                    (int)slot.MinimumBid,
                    (int)slot.BlitzPrice,
                    slot.IntProperties.Select(p => new IntItemPropertyGetItemResponse(p.PropertyValue, p.PropertyName.Name)),
                    slot.FloatProperties.Select(p => new FloatItemPropertyGetItemResponse(p.PropertyValue, p.PropertyName.Name)),
                    slot.StringProperties.Select(p => new StringItemPropertyGetItemResponse(p.PropertyValue, p.PropertyName.Name)),
                    slot.BoolProperties.Select(p => new BoolItemPropertyGetItemResponse(p.PropertyValue, p.PropertyName.Name)),
                    slot.Images.Select(image => image.SmallImagePath).FirstOrDefault()/*currently we have only 1 image per slot*/)
                )
                .AsNoTracking()
                .ToListAsync();

                itemInfo.SetAuctionSlots(slotsInfo.OrderBy(slot => slot.SlotNumber));
            }
            #endregion

            #region paymentInfo
            var paymentInfo = new ItemTradingGetPaymentInfoResponse();
            var paymentMethodsAsString = new List<string>();
            foreach (var paymentMethod in item.PaymentMethods)
            {
                paymentMethodsAsString.Add(Translator.Translate(paymentMethod));
            }
            paymentInfo.SetInfo(paymentMethodsAsString);
            #endregion

            #region userInfo
            var userInfo = new ItemTradingGetUserInfoResponse();
            var user = await _userContext.Users
                .Where(user => user.Id == item.UserId)
                .Select(user => new
                {
                    user.UserName,
                    user.ShopTitle,
                    user.Rating,
                    user.IsUserAsShopOption,
                    user.ShopLogoSmallImage,
                    user.UserLogoSmallImage
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (user is not null)
            {
                userInfo.SetInfo(user.UserName!,
                    user.IsUserAsShopOption ? user.ShopTitle : user.UserName!,
                    user.Rating,
                    user.IsUserAsShopOption ? user.ShopLogoSmallImage : user.UserLogoSmallImage,
                    user.IsUserAsShopOption,
                    9999);
            }
            #endregion

            #region deliveryInfo
            var deliveryInfo = new ItemTradingGetDeliveryInfoResponse();
            deliveryInfo.SetInfo(item.DaysForShipment, ["111111111111111", "2222222222222222222", "3333333333333333"]);
            #endregion

            var res = new ItemTradingGetResponse(itemInfo, paymentInfo, userInfo, deliveryInfo);

            return new MethodResult<ItemTradingGetResponse>(res, [], Domain.CoreEnums.Enums.MethodResults.Ok);
        }

        public async Task<MethodResult<ItemTradingGetItemForModerationResponse>> GetItemForModerationAsync()
        {
            var item = await _catalogContext.ItemsTrading
                .Where(x => x.Status == Domain.CoreEnums.Enums.ItemTradingStatus.Moderation)
                .OrderBy(x => x.StatusChangingLastTime)
                .Select(x => new
                {
                    x.Id,
                    x.Title,
                    x.Description,
                    images = x.Images.Select(i => i.SmallImagePath),
                    x.SellingType,
                    x.AuctionEndingTime,
                    x.FreeQuantity,
                    x.MinPrice,
                    cateroryName = x.CatalogCategory.Name,
                    intProps = x.IntProperties.Select(p => new IntItemPropertyGetItemResponse(p.PropertyValue, p.PropertyName.Name)),
                    floatProps = x.FloatProperties.Select(p => new FloatItemPropertyGetItemResponse(p.PropertyValue, p.PropertyName.Name)),
                    stringProps = x.StringProperties.Select(p => new StringItemPropertyGetItemResponse(p.PropertyValue, p.PropertyName.Name)),
                    boolProps = x.BoolProperties.Select(p => new BoolItemPropertyGetItemResponse(p.PropertyValue, p.PropertyName.Name)),
                    x.DaysForShipment,
                    x.PaymentMethods,
                }
                )
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (item == null)
            {
                return new MethodResult<ItemTradingGetItemForModerationResponse>(null, ["Нет товаров для модерации"], Domain.CoreEnums.Enums.MethodResults.Conflict);
            }

            #region itemInfo
            var itemInfo = new ItemTradingGetItemInfoResponse(item.Id,
                item.Title,
                item.Description,
                item.images,
                item.SellingType,
                item.AuctionEndingTime,
                item.FreeQuantity,
                item.MinPrice,
                item.cateroryName,
                item.intProps,
                item.floatProps,
                item.stringProps,
                item.boolProps);

            if (itemInfo.SellingType == Domain.CoreEnums.Enums.SellingTypes.Auction)
            {
                var slotsInfo = await _catalogContext.ItemTradingAuctionSlots
                .Where(slot => slot.ItemId == item.Id)
                .Select(slot => new ItemTradingAuctionSlotInfoGetItemResponse(
                    slot.Id,
                    slot.Title,
                    slot.AuctionSlotNum,
                    slot.Description,
                    (int)slot.Price,
                    (int)slot.MinimumBid,
                    (int)slot.BlitzPrice,
                    slot.IntProperties.Select(p => new IntItemPropertyGetItemResponse(p.PropertyValue, p.PropertyName.Name)),
                    slot.FloatProperties.Select(p => new FloatItemPropertyGetItemResponse(p.PropertyValue, p.PropertyName.Name)),
                    slot.StringProperties.Select(p => new StringItemPropertyGetItemResponse(p.PropertyValue, p.PropertyName.Name)),
                    slot.BoolProperties.Select(p => new BoolItemPropertyGetItemResponse(p.PropertyValue, p.PropertyName.Name)),
                    slot.Images.Select(image => image.SmallImagePath).FirstOrDefault()/*currently we have only 1 image per slot*/)
                )
                .AsNoTracking()
                .ToListAsync();

                itemInfo.SetAuctionSlots(slotsInfo.OrderBy(slot => slot.SlotNumber));
            }
            #endregion

            #region paymentInfo
            var paymentInfo = new ItemTradingGetPaymentInfoResponse();
            var paymentMethodsAsString = new List<string>();
            foreach (var paymentMethod in item.PaymentMethods)
            {
                paymentMethodsAsString.Add(Translator.Translate(paymentMethod));
            }
            paymentInfo.SetInfo(paymentMethodsAsString);
            #endregion

            #region deliveryInfo
            var deliveryInfo = new ItemTradingGetDeliveryInfoResponse();
            deliveryInfo.SetInfo(item.DaysForShipment, ["111111111111111", "2222222222222222222", "3333333333333333"]);
            #endregion

            var res = new ItemTradingGetItemForModerationResponse(itemInfo, paymentInfo, deliveryInfo);

            return new MethodResult<ItemTradingGetItemForModerationResponse>(res, [], Domain.CoreEnums.Enums.MethodResults.Ok);
        }

        public async Task<MethodResult<ItemTradingTemplateResponse>> GetItemTemplateAsync(Guid userId, Guid itemId)
        {
            var itemInfo = await _catalogContext.ItemsTrading
                .Where(x => x.Id == itemId && x.Status == Domain.CoreEnums.Enums.ItemTradingStatus.Template)
                .Select(x => new ItemTradingTemplateResponse(
                        x.Id,
                        x.CatalogCategory.SystemName,
                        x.Title,
                        x.Description,
                        x.SellingType,
                        x.AuctionEndingTime,
                        x.MinPrice,
                        x.Images.Select(i => i.SmallImagePath),
                        x.IntProperties.Select(p => new IntPropertyResponse(p.PropertyValue, p.PropertyName.SystemName)),
                        x.FloatProperties.Select(p => new FloatPropertyResponse(p.PropertyValue, p.PropertyName.SystemName)),
                        x.StringProperties.Select(p => new StringPropertyResponse(p.SystemValue, p.PropertyValue, p.PropertyName.SystemName)),
                        x.BoolProperties.Select(p => new BoolPropertyResponse(p.SystemValue, p.PropertyValue, p.PropertyName.SystemName)),
                        x.ShipmentAddresses.Select(a => new AddressItemTemplateResponse(
                            a.Country,
                            a.City,
                            a.Region,
                            a.District,
                            a.Street,
                            a.Building,
                            a.Floor,
                            a.Flat,
                            a.PostIndex,
                            a.AddressTitle)),
                        x.PaymentMethods,
                        x.FreeQuantity,
                        x.DaysForShipment)
                )
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (itemInfo == null)
            {
                return new MethodResult<ItemTradingTemplateResponse>(null, [], Domain.CoreEnums.Enums.MethodResults.Conflict);
            }

            if (itemInfo.SellingType == Domain.CoreEnums.Enums.SellingTypes.Auction)
            {
                var slotsInfo = await _catalogContext.ItemTradingAuctionSlots
                .Where(slot => slot.ItemId == itemId)
                .Select(slot => new ItemTradingAuctionSlotInfoResponse(
                    slot.Title,
                    slot.AuctionSlotNum,
                    slot.Description,
                    slot.Price,
                    slot.MinimumBid,
                    slot.BlitzPrice,
                    slot.IntProperties.Select(p => new IntPropertyResponse(p.PropertyValue, p.PropertyName.SystemName)),
                    slot.FloatProperties.Select(p => new FloatPropertyResponse(p.PropertyValue, p.PropertyName.SystemName)),
                    slot.StringProperties.Select(p => new StringPropertyResponse(p.SystemValue, p.PropertyValue, p.PropertyName.SystemName)),
                    slot.BoolProperties.Select(p => new BoolPropertyResponse(p.SystemValue, p.PropertyValue, p.PropertyName.SystemName)),
                    slot.Images.Select(image => image.SmallImagePath).FirstOrDefault()/*currently we have only 1 image per slot*/)
                )
                .AsNoTracking()
                .ToListAsync();

                itemInfo.SetAuctionSlots(slotsInfo);
            }

            return new MethodResult<ItemTradingTemplateResponse>(itemInfo, [], Domain.CoreEnums.Enums.MethodResults.Ok);
        }

        /// <summary>
        /// Creates/updates item template. If itemId is not provided, creates new item(template)
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public async Task<MethodResult> CreateUpdateItemAsync(Guid userId, ItemTradingInfoRequest itemInfo)
        {
            ItemTrading item;

            var catalogCategory = await _catalogContext.CatalogCategories
                .Include(x => x.CatalogBoolPropertyNames)
                    .ThenInclude(x => x.Properties)
                .Include(x => x.CatalogStringPropertyNames)
                    .ThenInclude(x => x.Properties)
                .Include(x => x.CatalogFloatPropertyNames)
                    .ThenInclude(x => x.Properties)
                .Include(x => x.CatalogIntPropertyNames)
                    .ThenInclude(x => x.Properties)
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.SystemName == itemInfo.CatalogSystemName);

            if (catalogCategory == null) return new MethodResult(["Не найдена категория"], Domain.CoreEnums.Enums.MethodResults.Conflict);

            var user = await _userContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null) return new MethodResult(["Ошибка пользователя"], Domain.CoreEnums.Enums.MethodResults.Conflict);

            var existedItem = await _catalogContext.ItemsTrading
                .Include(x => x.Images)
                .Include(x => x.ShipmentAddresses)
                .FirstOrDefaultAsync(x => x.Id == itemInfo.ItemId);

            if (existedItem != null)
            {
                item = existedItem;

                if (item.UserId != userId) return new MethodResult(["Ошибка пользователя"], Domain.CoreEnums.Enums.MethodResults.Conflict); //if item is not owned by current user
            }

            else
            {
                item = new(catalogCategory, userId, itemInfo.Title, itemInfo.Description, Domain.CoreEnums.Enums.SellingTypes.Standard);

                _catalogContext.ItemsTrading.Add(item);
            }

            List<ItemTradingImage> oldImages = new(item.Images);//add pictures to delete

            List<ItemTradingImage> newImages = new();//add pictures and save them
            foreach(var imageBase64 in itemInfo.ImagesBase64)
            {
                using var stream = new MemoryStream(Convert.FromBase64String(imageBase64));
                var bigImageRes = await _imageService.SaveImageAsync(Domain.CoreEnums.Enums.ImagePurpose.BigImage, stream);
                var smallImageRes = await _imageService.SaveImageAsync(Domain.CoreEnums.Enums.ImagePurpose.SmallImage, stream);

                if (bigImageRes.Result == Domain.CoreEnums.Enums.MethodResults.Conflict || smallImageRes.Result == Domain.CoreEnums.Enums.MethodResults.Conflict)
                    return new MethodResult(["Ошибка сохранений изображений"], Domain.CoreEnums.Enums.MethodResults.Conflict);

                newImages.Add(new ItemTradingImage(item, bigImageRes.ResultEntity!, smallImageRes.ResultEntity!));
            }

            List<CatalogIntProperty> oldIntProps = new();
            List<CatalogFloatProperty> oldFloatProps = new();

            List<CatalogStringProperty> strProps = new();
            List<CatalogIntProperty> intProps = new();
            List<CatalogFloatProperty> floatProps = new();
            List<CatalogBoolProperty> boolProps = new();

            item.StringProperties.ForEach(x => x.RemoveItem(item));
            item.StringProperties.Clear();//clear all old props refs from item
            foreach (var prop in itemInfo.StringProperties)
            {
                var existedProp = await _catalogContext.CatalogItemStringProperties.FirstOrDefaultAsync(x => x.SystemValue == prop.SystemValue); //try to find prop from dto 
                if (existedProp != null) { strProps.Add(existedProp); } //add it to new props list
            }

            item.BoolProperties.ForEach(x => x.RemoveItem(item));
            item.BoolProperties.Clear();//clear all old props refs from item
            foreach (var prop in itemInfo.BoolProperties)
            {
                var existedProp = await _catalogContext.CatalogItemBoolProperties.FirstOrDefaultAsync(x => x.SystemValue == prop.SystemValue); //try to find prop from dto 
                if (existedProp != null) { boolProps.Add(existedProp); } //add it to new props list
            }


            foreach (var prop in itemInfo.IntProperties)
            {
                var propName = catalogCategory.CatalogIntPropertyNames.FirstOrDefault(x => x.SystemName == prop.PropNameSystemName);//find propName
                if (propName == null) { return new MethodResult([$"Не найдено имя характеристики"], Domain.CoreEnums.Enums.MethodResults.Conflict); }

                item.IntProperties
                    .Where(x => x.PropertyName.SystemName == prop.PropNameSystemName)
                    .ToList()
                    .ForEach(x => { propName.RemoveProperty(x); oldIntProps.Add(x);}); // remove old value from propName

                var newProp = new CatalogIntProperty(prop.Value, item, propName);
                intProps.Add(newProp);//create and add new prop to new List
            }

            foreach (var prop in itemInfo.FloatProperties)
            {
                var propName = catalogCategory.CatalogFloatPropertyNames.FirstOrDefault(x => x.SystemName == prop.PropNameSystemName);//find propName
                if (propName == null) { return new MethodResult([$"Не найдено имя характеристики"], Domain.CoreEnums.Enums.MethodResults.Conflict); }

                item.FloatProperties
                    .Where(x => x.PropertyName.SystemName == prop.PropNameSystemName)
                    .ToList()
                    .ForEach(x => { propName.RemoveProperty(x); oldFloatProps.Add(x); }); // remove old value from propName

                var newProp = new CatalogFloatProperty(prop.Value, item, propName);
                floatProps.Add(newProp);//create and add new prop to new List
            }

            item.IntProperties.Clear();
            item.FloatProperties.Clear();

            var oldAddresses = await _catalogContext.Addresses
                .Include(a => a.Item)
                .Where(a => a.Item.Id == item.Id)
                .ToListAsync();

            List<ItemAddress> newAddresses = [];
            foreach (var address in itemInfo.Addresses)
            {
                newAddresses.Add(new ItemAddress(item,
                    address.AddressTitle,
                    address.Country,
                    address.City,
                    address.Region,
                    address.District,
                    address.Street,
                    address.Building,
                    address.Floor,
                    address.Flat,
                    address.PostIndex));
            };

            var res = item.SetSimpleItemInfo(itemInfo.Title,
                itemInfo.Description,
                itemInfo.Price,
                strProps,
                intProps,
                floatProps,
                boolProps,
                newAddresses,
                itemInfo.PaymentMethods.ToList(),
                newImages,
                itemInfo.Quantity);

            _catalogContext.CatalogItemIntProperties.RemoveRange(oldIntProps);
            _catalogContext.CatalogItemFloatProperties.RemoveRange(oldFloatProps);

            if (res.Result == Domain.CoreEnums.Enums.MethodResults.Conflict) return new MethodResult(res.Messages, Domain.CoreEnums.Enums.MethodResults.Conflict);

            _catalogContext.Addresses.RemoveRange(oldAddresses);

            //delete images from db and disk
            _catalogContext.ItemsTradingsImages.RemoveRange(oldImages);
            oldImages.ForEach(async x => { await _imageService.DeleteImageAsync(x.BigImagePath); await _imageService.DeleteImageAsync(x.SmallImagePath); });

            await _catalogContext.SaveChangesAsync();

            return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Ok);
        }

        public async Task<MethodResult> CreateUpdateAuctionItemAsync(Guid userId, ItemTradingAuctionInfoRequest itemInfo)
        {
            ItemTrading item;
            List<ItemTradingAuctionSlot> existedSlots = new();
            List<ItemTradingAuctionSlot> newSlots = new();

            var catalogCategory = await _catalogContext.CatalogCategories
                .Include(x => x.CatalogBoolPropertyNames)
                    .ThenInclude(x => x.Properties)
                .Include(x => x.CatalogStringPropertyNames)
                    .ThenInclude(x => x.Properties)
                .Include(x => x.CatalogFloatPropertyNames)
                    .ThenInclude(x => x.Properties)
                .Include(x => x.CatalogIntPropertyNames)
                    .ThenInclude(x => x.Properties)
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.SystemName == itemInfo.CatalogSystemName);

            if (catalogCategory == null) return new MethodResult(["Категория каталога не найдена"], Domain.CoreEnums.Enums.MethodResults.Conflict);

            var user = await _userContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null) return new MethodResult(["Ошибка пользователя"], Domain.CoreEnums.Enums.MethodResults.Conflict);

            var existedItem = await _catalogContext.ItemsTrading
                .Include(x => x.Images)
                .Include(x => x.IntProperties)
                .Include(x => x.FloatProperties)
                .Include(x => x.StringProperties)
                .Include(x => x.BoolProperties)
                .FirstOrDefaultAsync(x => x.Id == itemInfo.ItemId);

            if (existedItem != null)
            {
                item = existedItem;

                if (item.UserId != userId) return new MethodResult(["Ошибка пользователя"], Domain.CoreEnums.Enums.MethodResults.Conflict); //if item is not owned by current user

                if (existedItem.SellingType == Domain.CoreEnums.Enums.SellingTypes.Auction)
                {
                    existedSlots = await _catalogContext.ItemTradingAuctionSlots
                        .Include(slot => slot.Images)
                        .Include(x => x.IntProperties)
                        .Include(x => x.FloatProperties)
                        .Include(x => x.StringProperties)
                        .Include(x => x.BoolProperties)
                        .Where(x => x.ItemId == item.Id).ToListAsync();
                }
            }

            else
            {
                item = new(catalogCategory, userId, itemInfo.Title, itemInfo.Description, Domain.CoreEnums.Enums.SellingTypes.Auction);

                _catalogContext.ItemsTrading.Add(item);
            }

            List<ItemTradingImage> oldImages = new(item.Images);//add pictures to delete
            List<ItemTradingSlotImage> oldSlotImages = new(existedSlots.SelectMany(slot => slot.Images));//add slots pictures to delete

            //slots work
            foreach (var slot in itemInfo.Slots)
            {
                List<CatalogIntProperty> oldIntProps = new();
                List<CatalogFloatProperty> oldFloatProps = new();

                List<CatalogStringProperty> strProps = new();
                List<CatalogIntProperty> intProps = new();
                List<CatalogFloatProperty> floatProps = new();
                List<CatalogBoolProperty> boolProps = new();

                foreach (var prop in slot.StringProperties)
                {
                    var existedProp = await _catalogContext.CatalogItemStringProperties.FirstOrDefaultAsync(x => x.SystemValue == prop.SystemValue); //try to find prop from dto 
                    if (existedProp != null) { strProps.Add(existedProp); } //add it to new props list
                }

                foreach (var prop in slot.BoolProperties)
                {
                    var existedProp = await _catalogContext.CatalogItemBoolProperties.FirstOrDefaultAsync(x => x.SystemValue == prop.SystemValue); //try to find prop from dto 
                    if (existedProp != null) { boolProps.Add(existedProp); } //add it to new props list
                }


                foreach (var prop in slot.IntProperties)
                {
                    var propName = catalogCategory.CatalogIntPropertyNames.FirstOrDefault(x => x.SystemName == prop.PropNameSystemName);//find propName
                    if (propName == null) { return new MethodResult([$"Не найдено имя характеристики"], Domain.CoreEnums.Enums.MethodResults.Conflict); }

                    item.IntProperties
                    .Where(x => x.PropertyName.SystemName == prop.PropNameSystemName)
                    .ToList()
                    .ForEach(x => { propName.RemoveProperty(x); oldIntProps.Add(x); }); // remove old value from propName and add to list for removal
                    

                    var newProp = new CatalogIntProperty(prop.Value, item, propName);
                    intProps.Add(newProp);//create and add new prop to new List
                }

                foreach (var prop in slot.FloatProperties)
                {
                    var propName = catalogCategory.CatalogFloatPropertyNames.FirstOrDefault(x => x.SystemName == prop.PropNameSystemName);//find propName
                    if (propName == null) { return new MethodResult([$"Не найдено имя характеристики"], Domain.CoreEnums.Enums.MethodResults.Conflict); }

                    item.FloatProperties
                    .Where(x => x.PropertyName.SystemName == prop.PropNameSystemName)
                    .ToList()
                    .ForEach(x => { propName.RemoveProperty(x); oldFloatProps.Add(x); }); // remove old value from propName and add to list for removal

                    var newProp = new CatalogFloatProperty(prop.Value, item, propName);
                    floatProps.Add(newProp);//create and add new prop to new List
                }

                var newSlot = new ItemTradingAuctionSlot(catalogCategory,
                    item.Id,
                    slot.Title,
                    slot.Description,
                    slot.Price,
                    slot.MinimumBid,
                    slot.BlitzPrice,
                    itemInfo.AuctionEndingTime,
                    slot.SlotNumber);

                using var stream = new MemoryStream(Convert.FromBase64String(slot.ImageBase64));
                var bigImageRes = await _imageService.SaveImageAsync(Domain.CoreEnums.Enums.ImagePurpose.BigImage, stream);
                var smallImageRes = await _imageService.SaveImageAsync(Domain.CoreEnums.Enums.ImagePurpose.SmallImage, stream);

                if (bigImageRes.Result == Domain.CoreEnums.Enums.MethodResults.Conflict || smallImageRes.Result == Domain.CoreEnums.Enums.MethodResults.Conflict)
                    return new MethodResult(["Ошибка сохранения изображений"], Domain.CoreEnums.Enums.MethodResults.Conflict);

                
                var newSlotImage = new ItemTradingSlotImage(newSlot, bigImageRes.ResultEntity!, smallImageRes.ResultEntity!);


                newSlot.SetInfo(slot.Description,
                    slot.Price,
                    slot.MinimumBid,
                    slot.BlitzPrice,
                    itemInfo.AuctionEndingTime,
                    slot.SlotNumber,
                    [newSlotImage]/*currently we have only 1 image per slot*/,
                    strProps, floatProps, intProps, boolProps);

                newSlots.Add(newSlot);

                _catalogContext.ItemsTradingsSlotsImages.Add(newSlotImage);
                _catalogContext.ItemTradingAuctionSlots.Add(newSlot);

                _catalogContext.CatalogItemIntProperties.RemoveRange(oldIntProps);
                _catalogContext.CatalogItemFloatProperties.RemoveRange(oldFloatProps);
            }

            List<ItemTradingImage> newImages = [];//add pictures and save them
            foreach (var imageBase64 in itemInfo.ImagesBase64)
            {
                using var stream = new MemoryStream(Convert.FromBase64String(imageBase64));
                var bigImageRes = await _imageService.SaveImageAsync(Domain.CoreEnums.Enums.ImagePurpose.BigImage, stream);
                var smallImageRes = await _imageService.SaveImageAsync(Domain.CoreEnums.Enums.ImagePurpose.SmallImage, stream);

                if (bigImageRes.Result == Domain.CoreEnums.Enums.MethodResults.Conflict || smallImageRes.Result == Domain.CoreEnums.Enums.MethodResults.Conflict)
                    return new MethodResult(["Ошибка сохранения изображений"], Domain.CoreEnums.Enums.MethodResults.Conflict);

                newImages.Add(new ItemTradingImage(item, bigImageRes.ResultEntity!, smallImageRes.ResultEntity!));
            }

            //finally clear all item props
            item.StringProperties.ForEach(x => x.RemoveItem(item));
            item.StringProperties.Clear();

            item.BoolProperties.ForEach(x => x.RemoveItem(item));
            item.BoolProperties.Clear();

            item.IntProperties.Clear();
            item.FloatProperties.Clear();

            //collect all props from all slots
            var allStringProps = newSlots.SelectMany(x => x.StringProperties).DistinctBy(x => x.SystemValue).ToList();
            var allBoolProps = newSlots.SelectMany(x => x.BoolProperties).DistinctBy(x => x.SystemValue).ToList();
            var allIntProps = newSlots.SelectMany(x => x.IntProperties).DistinctBy(x => x.PropertyValue).ToList();
            var allFloatProps = newSlots.SelectMany(x => x.FloatProperties).DistinctBy(x => x.PropertyValue).ToList();

            //find max and min start price
            var minPrice = newSlots.Select(x => x.Price).Min();
            var maxPrice = newSlots.Select(x => x.Price).Max();

            var oldAddresses = await _catalogContext.Addresses
                .Include(a => a.Item)
                .Where(a => a.Item.Id == item.Id)
                .ToListAsync();

            List<ItemAddress> newAddresses = [];
            foreach (var address in itemInfo.Addresses)
            {
                newAddresses.Add(new ItemAddress(item,
                    address.AddressTitle,
                    address.Country,
                    address.City,
                    address.Region,
                    address.District,
                    address.Street,
                    address.Building,
                    address.Floor,
                    address.Flat,
                    address.PostIndex));
            };


            var res = item.SetAuctionItemInfo(itemInfo.Title,
                itemInfo.Description,
                minPrice,
                maxPrice,
                allStringProps, allIntProps, allFloatProps, allBoolProps,
                newAddresses,
                itemInfo.PaymentMethods.ToList(),
                newImages,
                itemInfo.AuctionEndingTime);

            if (res.Result == Domain.CoreEnums.Enums.MethodResults.Conflict) return new MethodResult(res.Messages, Domain.CoreEnums.Enums.MethodResults.Conflict);

            _catalogContext.Addresses.RemoveRange(oldAddresses);

            //delete images from db and disk
            _catalogContext.ItemsTradingsImages.RemoveRange(oldImages);
            oldImages.ForEach(async x => { await _imageService.DeleteImageAsync(x.BigImagePath); await _imageService.DeleteImageAsync(x.SmallImagePath); });

            _catalogContext.ItemsTradingsSlotsImages.RemoveRange(oldSlotImages);
            oldSlotImages.ForEach(async x => { await _imageService.DeleteImageAsync(x.BigImagePath); await _imageService.DeleteImageAsync(x.SmallImagePath); });

            _catalogContext.ItemTradingAuctionSlots.RemoveRange(existedSlots);//delete existed slots

            await _catalogContext.SaveChangesAsync();

            return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Ok);
        }

        public async Task<MethodResult> DeleteAsync(Guid userId, Guid itemId)
        {
            return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Ok);
        }
    }
}
