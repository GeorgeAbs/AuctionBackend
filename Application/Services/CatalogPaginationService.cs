using Application.Extensions;
using Domain.BackendResponses;
using Domain.Common.Filters;
using Domain.Common.Sorting;
using Domain.Entities.Items.ItemTrading;
using Domain.Interfaces.DbContexts;
using Domain.Interfaces.Services.CatalogPaginationService;
using Domain.Interfaces.Services.CatalogPaginationService.Dto;
using Domain.Interfaces.Services.CatalogPaginationService.Dto.Filtration;
using Domain.Interfaces.Services.CatalogPaginationService.Dto.Filtration.Props.Responce;
using Domain.Interfaces.Services.CatalogPaginationService.Dto.Filtration.PropsNames.Responce;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using static Domain.CoreEnums.Enums;

namespace Application.Services
{
    public class CatalogPaginationService : ICatalogPaginationService
    {
        private readonly ICatalogDbContext _context;
        private readonly IDistributedCache _cache;

        private static JsonSerializerOptions options = new JsonSerializerOptions()
        {
            //ReferenceHandler = ReferenceHandler.Preserve,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };

        public CatalogPaginationService(ICatalogDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<MethodResult<CatalogPaginationResponse>> GetPaginationViewModelByFilterAsync(CatalogFilterRequest filter)
        {
            var category = await _context.CatalogCategories
                .Where(x => x.SystemName == filter.CategoryName)
                .AsNoTracking()
                .Select(x => new { x.Id, x.SystemName, x.Name })
                .FirstAsync();


            var intPropNames = await _context.CatalogIntPropertyNames
                    .Select(x => new { x.Id, catId = x.CatalogCategory.Id, x.SystemName, minValue = x.Properties.Select(p => p.PropertyValue).Min(), maxValue = x.Properties.Select(p => p.PropertyValue).Max() })
                    .AsNoTracking()
                    .Where(x => x.catId == category.Id)
                    .ToListAsync();

            var floatPropNames = await _context.CatalogFloatPropertyNames
                    .Select(x => new { x.Id, catId = x.CatalogCategory.Id, x.SystemName, minValue = x.Properties.Select(p => p.PropertyValue).Min(), maxValue = x.Properties.Select(p => p.PropertyValue).Max() })
                    .AsNoTracking()
                    .Where(x => x.catId == category.Id)
                    .ToListAsync();

            await Console.Out.WriteLineAsync(JsonSerializer.Serialize(filter, options));

            var items = _context.ItemsTrading.AsNoTracking();

            items = items.Where(i => i.CatalogCategory.Id == category.Id);

            items = items.Where(i => i.MinPrice >= filter.PriceMin && i.MaxPrice <= filter.PriceMax);

            items = items.Where(i => i.Status == ItemTradingStatus.Published || i.Status == ItemTradingStatus.LockedByAuction); //важно. вынести в отельный список и использовать predicate builder


            foreach (var prop in filter.FilterFloatNames)
            {
                var predicate = PredicateBuilder.New<ItemTrading>();

                predicate = predicate.Or(x => x.FloatProperties.Any(p => p.PropertyName.SystemName == prop.CatalogNameSystemName &&
                                                                            p.PropertyValue >= prop.MinValue &&
                                                                             p.PropertyValue <= prop.MaxValue));

                items = items.Where(predicate);
            }

            foreach (var prop in filter.FilterIntNames)
            {
                var predicate = PredicateBuilder.New<ItemTrading>();

                var name = intPropNames.First(x => x.SystemName == prop.CatalogNameSystemName);

                predicate = predicate.Or(x => x.IntProperties.Any(p => p.PropertyName.Id == name.Id &&
                                                                            p.PropertyValue >= prop.MinValue &&
                                                                             p.PropertyValue <= prop.MaxValue));

                items = items.Where(predicate);
            }

            foreach (var prop in filter.FilterStringNames)
            {
                var predicate = PredicateBuilder.New<ItemTrading>();

                predicate = predicate.Or(x => x.StringProperties.Any(p => prop.PropsSystemValues.Contains(p.SystemValue)));

                items = items.Where(predicate);
            }

            foreach (var prop in filter.FilterBoolNames)
            {
                var predicate = PredicateBuilder.New<ItemTrading>();

                predicate = predicate.Or(x => x.BoolProperties.Any(p => prop.PropsSystemValues.Contains(p.SystemValue)));

                items = items.Where(predicate);
            }

            var promotedItems = await items
                .Where(item => item.IsPromotedByPriority)
                .OrderBy(x => EF.Functions.Random())
                .Take(4)
                .Select(x => new ItemTradingPaginationResponse(x.Id, x.Title, x.MinPrice, x.MaxPrice, x.AuctionEndingTime, x.SellingType, x.DesignPromotionType))
                .ToListAsync();

            switch (filter.SortingMethod)
            {
                case SortingMethods.PriceAsc:
                    items = items.OrderBy(x => x.MinPrice).ThenBy(x => x.Title);
                    break;

                case SortingMethods.PriceDesc:
                    items = items.OrderByDescending(x => x.MinPrice).ThenBy(x => x.Title);
                    break;

                case SortingMethods.DateAsc:
                    items = items.OrderBy(x => x.LastModifiedTime).ThenBy(x => x.Title);
                    break;

                case SortingMethods.DateDesc:
                    items = items.OrderByDescending(x => x.LastModifiedTime).ThenBy(x => x.Title);
                    break;
            }

            var totalItems = await items.CountAsync();

            items =  items
                .Skip(filter.PageSize * (filter.PageNumber - 1))
                .Take(filter.PageSize);

            var takenItems = await items
                .Select(x => new ItemTradingPaginationResponse(x.Id, x.Title, x.MinPrice,x.MaxPrice, x.AuctionEndingTime, x.SellingType, x.DesignPromotionType))
                .ToListAsync();

            var itemsIds = takenItems.Select(x => x.Id);
            var imgs = await _context.ItemsTradingsImages.Select(x => new {x.SmallImagePath, ownerId = x.OwnerEntity.Id}).Where(x => itemsIds.Contains(x.ownerId)).ToListAsync();
            
            foreach (var item in takenItems)
            {
                var img = imgs.Where(x => x.ownerId == item.Id).FirstOrDefault();
                if (img is not null) item.ImageUrl = img.SmallImagePath;
            }


            var newFilter = totalItems == 0 ||
                (!filter.FilterBoolNames.Any() && !filter.FilterStringNames.Any() && !filter.FilterFloatNames.Any() && !filter.FilterIntNames.Any())
                ? new CatalogFilterResponse(category.Name)
                : await GetFilterAsync(items, category.Name);

            return new MethodResult<CatalogPaginationResponse>(new(promotedItems,takenItems, newFilter, filter.PageNumber, totalItems, filter.PageSize), [], MethodResults.Ok);
        }

        public async Task<MethodResult<CatalogPaginationResponse>> GetCategoryFullFilteringAsync(CatalogFilterRequest filter)
        {
            var category = await _context.CatalogCategories
                .Where(c => c.SystemName == filter.CategoryName)
                .Select(x => new { x.Name, x.SystemName})
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (category is null)
            {
                return new MethodResult<CatalogPaginationResponse>(null, ["Категория не найдена"], MethodResults.Conflict);
            }

            var items = FirstlyFilterItems();

            items = items.Where(i => i.CatalogCategory.SystemName == filter.CategoryName);

            items = items.Where(i => i.MinPrice >= filter.PriceMin && i.MaxPrice <= filter.PriceMax);

            if (Filters.SellingTypesFilterForChecking.Contains(filter.SellingType))
            {
                items = items.Where(i => i.SellingType == filter.SellingType);
            }

            var promotedItems = await items
                .Where(item => item.IsPromotedByPriority)
                .OrderBy(x => EF.Functions.Random())
                .Take(4)
                .Select(x => new ItemTradingPaginationResponse(x.Id, x.Title, x.MinPrice, x.MaxPrice, x.AuctionEndingTime, x.SellingType, x.DesignPromotionType))
                .ToListAsync();

            var totalItems = await items.CountAsync();

            items = Sorting.SortItemsTrading(items, filter.SortingMethod);

            var takenItems = await items
                .Skip(filter.PageSize * (filter.PageNumber - 1))
                .Take(filter.PageSize)
                .Select(x => new ItemTradingPaginationResponse(x.Id, x.Title, x.MinPrice,x.MaxPrice, x.AuctionEndingTime, x.SellingType, x.DesignPromotionType))
                .ToListAsync();

            var itemsIds = takenItems.Select(x => x.Id);
            var imgs = await _context.ItemsTradingsImages.Select(x => new { x.SmallImagePath, ownerId = x.OwnerEntity.Id }).Where(x => itemsIds.Contains(x.ownerId)).ToListAsync();

            foreach (var item in takenItems)
            {
                var img = imgs.FirstOrDefault(x => x.ownerId == item.Id);
                if (img is not null) item.ImageUrl = img.SmallImagePath;
            }


            var newFilter = await GetFullCategoryFilterAsync(items, category.SystemName, category.Name);

            return new MethodResult<CatalogPaginationResponse>(new(promotedItems,takenItems, newFilter, filter.PageNumber, totalItems, filter.PageSize), [], MethodResults.Ok);
        }

        public async Task<CatalogFilterResponse> GetFilterAsync(IQueryable<ItemTrading> items, string categoryName)
        {
            var stringPropsNames = await _context.CatalogStringPropertyNames
               .SelectMany(x => x.Properties.Select(p => new { p.Id, p.SystemValue, p.PropertyName.SystemName }))
               .Distinct()
               .Where(x => items.SelectMany(item => item.StringProperties.Select(prop => prop.Id)).Contains(x.Id))
               .GroupBy(x => x.SystemName)
               .Select(g => new FilterStringPropertyNameResponse(
                           g.Key, "", g.Select(p => new FilterStringProperty(p.SystemValue, ""))))
               .ToListAsync();

            var boolPropsNames = await _context.CatalogBoolPropertyNames
               .SelectMany(x => x.Properties.Select(p => new { p.Id, p.SystemValue, p.PropertyName.SystemName }))
               .Distinct()
               .Where(x => items.SelectMany(item => item.StringProperties.Select(prop => prop.Id)).Contains(x.Id))
               .GroupBy(x => x.SystemName)
               .Select(g => new FilterBoolPropertyNameResponse(
                           g.Key, "", g.Select(p => new FilterBoolProperty(p.SystemValue, ""))))
               .ToListAsync();

            return new CatalogFilterResponse(categoryName,
                null,
                boolPropsNames,
                stringPropsNames);
        }

        public async Task<CatalogFilterResponse> GetFullCategoryFilterAsync(IQueryable<ItemTrading> items, string categoryName, string categoryDisplayName)
        {
            var filter = await _cache.GetStringByCompKeyAsync(typeof(CatalogFilterResponse), categoryName);

            if (filter is not null) 
            {
                var filterObj = JsonSerializer.Deserialize<CatalogFilterResponse>(filter, options);

                await Console.Out.WriteLineAsync(filter);

                await Console.Out.WriteLineAsync("name:" + filterObj.CategoryDisplayName);

                if (filterObj is not null) return filterObj;
            }
            

            var minPrice = await _context.ItemsTrading
                .Where(x => x.Status == ItemTradingStatus.Published || x.Status == ItemTradingStatus.LockedByAuction && x.CatalogCategory.SystemName == categoryName)
                .MinAsync(x => (int?)x.MinPrice) ?? 0;

            var maxPrice = await _context.ItemsTrading
                .Where(x => x.Status == ItemTradingStatus.Published || x.Status == ItemTradingStatus.LockedByAuction && x.CatalogCategory.SystemName == categoryName)
                .MaxAsync(x => (int?)x.MaxPrice) ?? 0;

            var stringPropsNames = await _context.CatalogStringPropertyNames
                .SelectMany(x => x.Properties.Select(p => new { p.Id, p.SystemValue,p.PropertyValue, propVisibility = p.IsVisible, p.PropertyName.SystemName, nameVisibility = p.PropertyName.IsVisible, p.PropertyName.Name }))
                .Where(x => /*x.nameVisibility && x.propVisibility && */items.SelectMany(item => item.StringProperties.Select(prop => prop.Id)).Contains(x.Id))
                .Distinct()
                .GroupBy(x => new { x.SystemName, x.Name })
                .Select(g => new FilterStringPropertyNameResponse(
                            g.Key.SystemName, g.Key.Name, g.Select(p => new FilterStringProperty(p.SystemValue, p.PropertyValue))))
                .ToListAsync();

             var boolPropsNames = await _context.CatalogBoolPropertyNames
                .SelectMany(x => x.Properties.Select(p => new { p.Id, p.SystemValue, p.PropertyValue, propVisibility = p.IsVisible, p.PropertyName.SystemName,nameVisibility =  p.PropertyName.IsVisible, p.PropertyName.Name }))
                .Where(x => /*x.nameVisibility && x.propVisibility && */items.SelectMany(item => item.BoolProperties.Select(prop => prop.Id)).Contains(x.Id))
                .Distinct()
                .GroupBy(x => new { x.SystemName, x.Name })
                .Select(g => new FilterBoolPropertyNameResponse(
                            g.Key.SystemName, g.Key.Name, g.Select(p => new FilterBoolProperty(p.SystemValue, p.PropertyValue))))
                .ToListAsync();

            var intPropNames = await items
                .SelectMany(item => item.IntProperties.Select(prop => new { prop.PropertyValue, prop.PropertyName.SystemName, prop.PropertyName.Name }))
                .GroupBy(prop => new { prop.SystemName, prop.Name })
                .Select(g => new FilterIntPropertyNameResponse(
                                 g.Key.SystemName, g.Key.Name, g.Select(value => value.PropertyValue).Min(), g.Select(value => value.PropertyValue).Max()))
                .ToListAsync();

            var floatPropNames = await items
                .SelectMany(item => item.FloatProperties.Select(prop => new { prop.PropertyValue, prop.PropertyName.SystemName, prop.PropertyName.Name }))
                .GroupBy(prop => new { prop.SystemName, prop.Name })
                .Select(g => new FilterFloatPropertyNameResponse(
                                 g.Key.SystemName, g.Key.Name, g.Select(value => value.PropertyValue).Min(), g.Select(value => value.PropertyValue).Max()))
                .ToListAsync();

            var resFilter = new CatalogFilterResponse(categoryDisplayName,
                new CatalogPriceFilterResponse(minPrice, maxPrice),
                boolPropsNames,
                stringPropsNames,
                floatPropNames,
                intPropNames);

            var serializedFilter = JsonSerializer.Serialize(resFilter, options);

            await _cache.SetStringByCompKeyAsync(typeof(CatalogFilterResponse), categoryName, serializedFilter);

            return resFilter;
        }

        public async Task<IEnumerable<ItemTradingPaginationResponse>> GetPopularItemsAsync()
        {
            var items = FirstlyFilterItems();

            var selectedItems = await items
                .Where(item => item.IsPromotedByPriority)
                .OrderBy(x => EF.Functions.Random())
                .Take(20)
                .Select(x => new ItemTradingPaginationResponse(x.Id, x.Title, x.MinPrice, x.MaxPrice, x.AuctionEndingTime, x.SellingType, x.DesignPromotionType))
                .ToListAsync();

            var itemsIds = selectedItems.Select(x => x.Id);
            var imgs = await _context.ItemsTradingsImages.Select(x => new { x.SmallImagePath, ownerId = x.OwnerEntity.Id }).Where(x => itemsIds.Contains(x.ownerId)).ToListAsync();

            foreach (var item in selectedItems)
            {
                var img = imgs.FirstOrDefault(x => x.ownerId == item.Id);
                if (img is not null) item.ImageUrl = img.SmallImagePath;
            }

            return selectedItems;
        }

        public async Task<IEnumerable<ItemTradingPaginationResponse>> GetNewItemsAsync()
        {
            var items = FirstlyFilterItems();

            var selectedItems = await items
                .OrderByDescending(x => x.CreationTime)
                .Take(1000)
                .OrderBy(x => EF.Functions.Random())
                .Take(20)
                .Select(x => new ItemTradingPaginationResponse(x.Id, x.Title, x.MinPrice, x.MaxPrice, x.AuctionEndingTime, x.SellingType, x.DesignPromotionType))
                .ToListAsync();

            var itemsIds = selectedItems.Select(x => x.Id);
            var imgs = await _context.ItemsTradingsImages.Select(x => new { x.SmallImagePath, ownerId = x.OwnerEntity.Id }).Where(x => itemsIds.Contains(x.ownerId)).ToListAsync();

            foreach (var item in selectedItems)
            {
                var img = imgs.FirstOrDefault(x => x.ownerId == item.Id);
                if (img is not null) item.ImageUrl = img.SmallImagePath;
            }

            return selectedItems;
        }

        private IQueryable<ItemTrading> FirstlyFilterItems()
        {
            var items = _context.ItemsTrading
               .AsNoTracking();

            items = items.Where(i => i.Status == ItemTradingStatus.Published || i.Status == ItemTradingStatus.LockedByAuction);

            return items;
        }
    }
}
