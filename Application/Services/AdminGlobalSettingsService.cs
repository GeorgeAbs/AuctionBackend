using Application.Extensions;
using Domain.BackendResponses;
using Domain.Constants;
using Domain.Interfaces.DbContexts;
using Domain.Interfaces.Services.AdminGlobalSettingsService;
using Domain.Interfaces.Services.AdminGlobalSettingsService.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.Services
{
    public class AdminGlobalSettingsService : IAdminGlobalSettingsService
    {
        public readonly ICatalogDbContext _catalogDbContext;
        public readonly IDistributedCache _cache;

        public AdminGlobalSettingsService(ICatalogDbContext catalogDbContext, IDistributedCache cache) 
        {
            _catalogDbContext = catalogDbContext;
            _cache = cache;
        }

        public async Task<MethodResult<StartPageDesignInfo>> GetStartPageDesignInfoAsync()
        {
            string? banner;

            var existedBanner = await _cache.GetStringByCompKeyAsync(typeof(string), RedisKeys.FIRST_BANNER);

            if (existedBanner != null) banner = existedBanner;

            else
            {
                banner = await _catalogDbContext.BannerImages
                .Where(banner => banner.BannerType == Domain.CoreEnums.Enums.BannerType.FistPageBanner)
                .Select(banner => banner.BigImagePath)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            }

            var startPageDesignInfo = new StartPageDesignInfo(banner);

            return new MethodResult<StartPageDesignInfo>(startPageDesignInfo, [], Domain.CoreEnums.Enums.MethodResults.Ok);
        }
    }
}
