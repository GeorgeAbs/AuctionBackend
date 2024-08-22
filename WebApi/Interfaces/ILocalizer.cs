using static Domain.CoreEnums.Enums;
using static WebApi.Constants.Enums;

namespace WebApi.Interfaces
{
    public interface ILocalizer
    {
        public void Initialize(Locale local);

        public string LocalizeSortings(SortingMethods sorting);

        public string LocalizeReviewCommentType(ReviewCommentsTypes sorting);

        public string LocalizeItemTradingStatus(ItemTradingStatus status);
    }
}
