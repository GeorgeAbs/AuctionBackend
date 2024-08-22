using Domain.BackendResponses;
using Domain.Interfaces.Services.AdminGlobalSettingsService.DTO;

namespace Domain.Interfaces.Services.AdminGlobalSettingsService
{
    public interface IAdminGlobalSettingsService
    {
        public Task<MethodResult<StartPageDesignInfo>> GetStartPageDesignInfoAsync();
    }
}
