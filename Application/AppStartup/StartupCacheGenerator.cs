using Application.Extensions;
using Domain.Interfaces.DbContexts;
using Domain.Interfaces.Services.CatalogPaginationService;
using Domain.Interfaces.Services.CatalogPaginationService.Dto.Filtration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Application.AppStartup
{
    public class StartupCacheGenerator(ICatalogDbContext catalogContext, IDistributedCache cache, ICatalogPaginationService catalogPaginationService)
    {
        private readonly ICatalogDbContext _catalogContext = catalogContext;
        private readonly IDistributedCache _cache = cache;
        private readonly ICatalogPaginationService _catalogPaginationService = catalogPaginationService;

        private static JsonSerializerOptions options = new JsonSerializerOptions()
        {
            //ReferenceHandler = ReferenceHandler.Preserve,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };
        public async Task GenerateCacheAsync()
        {
            var categories = await _catalogContext.CatalogCategories.ToListAsync();

            foreach (var category in categories)
            {
                var res = await _catalogPaginationService.GetCategoryFullFilteringAsync(new(1, 1, category.SystemName, 1, float.MaxValue, Domain.CoreEnums.Enums.SortingMethods.PriceAsc, [], [], [], [], Domain.CoreEnums.Enums.SellingTypes.All));

                var filter = res.ResultEntity!.Filter;

                var serializedFilter = JsonSerializer.Serialize(filter, options);

                await _cache.SetStringByCompKeyAsync(typeof(CatalogFilterResponse), category.SystemName, serializedFilter);
            }

        }
    }
}
