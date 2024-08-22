using Domain.CoreBindings;
using Domain.Entities.CatalogFiltering.PropertyItems;

namespace Domain.Entities.CatalogFiltering
{
    public class CatalogFilter : EntityBase
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 40;

        public Guid CatalogCategoryId { get; set; }

        public List<CatalogFilterStringProperties> ListCatalogFilterStringProperties { get; set; } = new();

        public List<CatalogFilterFloatProperty> ListCatalogFilterFloatProperties { get; set; } = new();

        public List<CatalogFilterIntProperties> ListCatalogFilterIntProperties { get; set; } = new();

        public CatalogFilterPropertyPriceFilter CatalogFilterPropertyPriceFilter { get; set; } = new();

        public string SortingBy { get; set; } = CatalogSorting.CREATION_DATE;

        public string SortingByDisplayedValue { get; set; } = CatalogSorting.CatalogSortingBinding[CatalogSorting.CREATION_DATE];

        public bool IsAscending { get; set; } = false;

        public string SortingDirectionDisplayedValue { get; set; } = CatalogSorting.CatalogSortingDirectionBinding[false];

        public Dictionary<string, string> SortingsBy { get; set; } = CatalogSorting.CatalogSortingBinding;

        public CatalogFilter() { }  ///нужно отдать список сортировок с возможными направлениями, а не одно значение! получить надо конкретную сортировку и ее направление
    }
}
