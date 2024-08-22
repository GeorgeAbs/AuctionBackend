using Domain.Entities.Items.ItemTrading;
using Domain.Interfaces.Services.CatalogPaginationService.Dto.Filtration;
using Domain.Interfaces.Services.CatalogPaginationService.Dto;
using Domain.BackendResponses;

namespace Domain.Interfaces.Services.CatalogPaginationService
{
    public interface ICatalogPaginationService
    {
        public Task<MethodResult<CatalogPaginationResponse>> GetPaginationViewModelByFilterAsync(CatalogFilterRequest filter);

        public Task<MethodResult<CatalogPaginationResponse>> GetCategoryFullFilteringAsync(CatalogFilterRequest filter);

        public Task<CatalogFilterResponse> GetFilterAsync(IQueryable<ItemTrading> items, string categoryName);

        public Task<IEnumerable<ItemTradingPaginationResponse>> GetPopularItemsAsync();

        public Task<IEnumerable<ItemTradingPaginationResponse>> GetNewItemsAsync();
    }
}
