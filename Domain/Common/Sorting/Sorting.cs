using Domain.Entities.Comments;
using Domain.Entities.Items.ItemTrading;
using Domain.Entities.Reviews;
using static Domain.CoreEnums.Enums;

namespace Domain.Common.Sorting
{
    public static class Sorting
    {
        /// <summary>
        /// Sorts IQueryable of itemsTrading by price or last time of status changing
        /// </summary>
        /// <param name="items"></param>
        /// <param name="sortingMethod"></param>
        /// <returns></returns>
        public static IQueryable<ItemTrading> SortItemsTrading(IQueryable<ItemTrading> items, SortingMethods sortingMethod)
        {
            return sortingMethod switch
            {
                SortingMethods.PriceAsc => items.OrderBy(x => x.MinPrice).ThenBy(x => x.Title),
                SortingMethods.PriceDesc => items.OrderByDescending(x => x.MinPrice).ThenBy(x => x.Title),
                SortingMethods.DateAsc => items.OrderBy(x => x.StatusChangingLastTime).ThenBy(x => x.Title),
                SortingMethods.DateDesc => items.OrderByDescending(x => x.StatusChangingLastTime).ThenBy(x => x.Title),
                _ => items,
            };
        }

        public static void SortComments(IQueryable<Comment> Comments, SortingMethods sortingMethod)
        {
            switch (sortingMethod)
            {
                case SortingMethods.DateAsc:
                    Comments.OrderBy(x => x.CreationTime); return;

                case SortingMethods.DateDesc:
                    Comments.OrderByDescending(x => x.CreationTime); return;
            }
        }

        public static void SortReviews(IQueryable<Review> Reviews, SortingMethods sortingMethod)
        {
            switch (sortingMethod)
            {
                case SortingMethods.DateAsc:
                    Reviews.OrderBy(x => x.CreationTime); return;

                case SortingMethods.DateDesc:
                    Reviews.OrderByDescending(x => x.CreationTime); return;
            }
        }
    }
}
