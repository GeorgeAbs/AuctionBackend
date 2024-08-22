using Domain.Interfaces;

namespace Domain.Common.Dto
{
    public class PaginationViewModel<TItems> : IPaginationViewModel<TItems> where TItems : class
    {
        public IEnumerable<TItems> ResponseEntities { get; private set; }

        public int PageNumber { get; private set; }

        public int TotalItems { get; private set; }

        public int TotalPages { get; private set; }

        public bool HasPreviousPage
        {
            get
            {
                return PageNumber > 1;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return PageNumber < TotalPages;
            }
        }

        public PaginationViewModel(IEnumerable<TItems> responseEntities, int pageNumber, int totalItems, int pageSize)
        {
            ResponseEntities = responseEntities;
            PageNumber = pageNumber;
            TotalItems = totalItems;
            TotalPages = (int)Math.Ceiling(totalItems / (float)pageSize);
        }
    }

    public class PaginationViewModel<TItems, TFilter> : IPaginationViewModel<TItems, TFilter> where TItems : class where TFilter : class
    {
        public IEnumerable<TItems> ResponseEntities { get; private set; }

        public TFilter Filter { get; private set; }

        public int PageNumber { get; private set; }

        public int TotalItems { get; private set; }

        public int TotalPages { get; private set; }

        public bool HasPreviousPage
        {
            get
            {
                return PageNumber > 1;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return PageNumber < TotalPages;
            }
        }

        public PaginationViewModel(IEnumerable<TItems> responseEntities, TFilter filter,  int pageNumber, int totalItems, int pageSize)
        {
            ResponseEntities = responseEntities;
            Filter = filter;
            PageNumber = pageNumber;
            TotalItems = totalItems;
            TotalPages = (int)Math.Ceiling(totalItems / (float)pageSize);
        }
    }

    public class PaginationWithPromotedItemsViewModel<TItems, TFilter> : PaginationViewModel<TItems, TFilter> where TItems : class where TFilter : class
    {
        public IEnumerable<TItems> PromotedResponseEntities { get; private set; }

        public PaginationWithPromotedItemsViewModel(IEnumerable<TItems> promotedResponseEntities,IEnumerable<TItems> responseEntities, TFilter filter, int pageNumber, int totalItems, int pageSize) : base(responseEntities, filter, pageNumber, totalItems, pageSize)
        {
            PromotedResponseEntities = promotedResponseEntities;
        }
    }


}
