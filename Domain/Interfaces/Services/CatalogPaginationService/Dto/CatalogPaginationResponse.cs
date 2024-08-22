using Domain.Common.Dto;
using Domain.Interfaces.Services.CatalogPaginationService.Dto.Filtration;

namespace Domain.Interfaces.Services.CatalogPaginationService.Dto
{
    public class CatalogPaginationResponse : PaginationWithPromotedItemsViewModel<ItemTradingPaginationResponse, CatalogFilterResponse>
    {
        public CatalogPaginationResponse(IEnumerable<ItemTradingPaginationResponse> promotedItems, IEnumerable<ItemTradingPaginationResponse> items,
            CatalogFilterResponse filter,
            int pageNumber,
            int totalItems,
            int pageSize) : base(promotedItems, items, filter, pageNumber, totalItems, pageSize) { }
    }
}
