namespace Domain.Interfaces.Services.AdminGlobalSettingsService.DTO
{
    public class StartPageDesignInfo
    {
        public string? BannerImageURL { get; private set; }

        public StartPageDesignInfo(string? bannerImageURL)
        {
            BannerImageURL = bannerImageURL;
        }
    }
}
