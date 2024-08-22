using Domain.Common.Filters;
using Domain.CoreEnums;
using WebApi.DTO.Common;
using WebApi.Interfaces;

namespace WebApi.DTO.CommentsReviewFilter.Response
{
    public class CommentsReviewFilterResponse
    {
        public IReadOnlyCollection<SortingItem> SortingItems { get; private set; } = Filters.SortingPriceDateFilter;

        public List<ReviewCommentsTypeItem> ReviewCommentsTypeItems { get; private set; } = new();

        public CommentsReviewFilterResponse(ILocalizer localizer, IReadOnlyCollection<Enums.ReviewCommentsTypes> reviewCommentsTypes) 
        {

            foreach (var type in reviewCommentsTypes)
            {
                ReviewCommentsTypeItems.Add(new(type, localizer.LocalizeReviewCommentType(type)));
            }
        }
    }
}
