namespace Domain.Interfaces
{
    public interface IPaginationViewModel<TItems> where TItems : class
    {
        public IEnumerable<TItems> ResponseEntities { get; }

        public int PageNumber { get; }

        public int TotalItems { get; }

        public int TotalPages { get; }

        public bool HasPreviousPage { get; }

        public bool HasNextPage { get; }
    }

    public interface IPaginationViewModel<TItems, TFilter> where TItems : class where TFilter : class
    {
        public IEnumerable<TItems> ResponseEntities { get; }

        public TFilter Filter { get; }

        public int PageNumber { get; }

        public int TotalItems { get; }

        public int TotalPages { get; }

        public bool HasPreviousPage { get; }

        public bool HasNextPage { get; }
    }
}
