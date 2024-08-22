using Domain.CoreBindings;

namespace Domain.Entities.CatalogFiltering
{
    public class CatalogFilterSortingBy
    {
        public string DisplayedValue { get; set; } = CatalogSorting.CatalogSortingBinding[CatalogSorting.CREATION_DATE];

        public string SortingBy { get; set; } = CatalogSorting.CREATION_DATE;

        public bool IsSelected { get; set; } = false;

        public CatalogFilterSortingBy() { }
    }
}
