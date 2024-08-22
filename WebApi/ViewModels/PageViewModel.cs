namespace WebApi.ViewModels
{
    public class PageViewModel(int totalItems, int pageNumber, int pageSize)
    {
        public int PageNumber { get; private set; } = pageNumber;
        public int TotalPages { get; private set; } = (int)Math.Ceiling(totalItems / (double)pageSize);

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
    }
}
