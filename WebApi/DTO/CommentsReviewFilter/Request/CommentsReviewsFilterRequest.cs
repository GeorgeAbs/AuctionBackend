using Application;
using Domain.CoreEnums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Domain.CoreEnums.Enums;

namespace WebApi.DTO.CommentsReviewFilter.Request
{
    public class CommentsReviewsFilterRequest
    {
        [FromQuery(Name = "page-number")]
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [FromQuery(Name = "page-size")]
        [Range(1, int.MaxValue)]
        public int PageSize { get; set; } = Settings.HISTORY_PAGE_SIZE;

        [FromQuery(Name = "sorting")]
        public SortingMethods Sorting { get; set; } = SortingMethods.DateAsc;

        [FromQuery(Name = "date-from")]
        public DateTime DateFrom { get; set; } = DateTime.MinValue;

        [FromQuery(Name = "date-to")]
        public DateTime DateTo { get; set; } = DateTime.MaxValue;

        [FromQuery(Name = "type")]
        public Enums.ReviewCommentsTypes ReviewCommentsType { get; set; } = ReviewCommentsTypes.My;
    }
}
