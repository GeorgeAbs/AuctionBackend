using Domain.Common.Filters;
using Domain.CoreEnums;
using WebApi.DTO.Common;
using WebApi.Interfaces;

namespace WebApi.DTO.HistoryFilter.Response
{
    public class HistoryFilterResponse
    {
        public IReadOnlyCollection<Domain.Common.Filters.SortingItem> SortingItems {  get; private set; } = Filters.SortingPriceDateFilter;

        public List<StatusItem> StatusItems { get; private set; } = new();

        public HistoryFilterResponse(ILocalizer localizer, IReadOnlyCollection<Enums.ItemTradingStatus> statuses)
        {
            foreach (var status in statuses)
            {
                StatusItems.Add(new StatusItem(status, localizer.LocalizeItemTradingStatus(status)));
            }
        }
    }
}
