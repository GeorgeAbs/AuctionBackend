using Application;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Domain.CoreEnums.Enums;

namespace WebApi.DTO.History.Requests
{
    public class HistoryFilterRequest
    {
        [FromQuery(Name = "page-number")]
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [FromQuery(Name = "page-size")]
        [Range(1, int.MaxValue)]
        public int PageSize { get; set; } = Settings.HISTORY_PAGE_SIZE;

        [FromQuery(Name = "sorting")]
        public SortingMethods Sorting { get; set; } = SortingMethods.DateDesc;

        [FromQuery(Name = "date-from")]
        public DateTime DateFrom { get; set; } = DateTime.MinValue;

        [FromQuery(Name = "date-to")]
        public DateTime DateTo { get; set; } = DateTime.MaxValue;

        [FromQuery(Name = "status")]
        public ItemTradingStatus ItemStatus { get; set; } = ItemTradingStatus.AllStatuses;
    }
}
