namespace Domain.Interfaces.Services.CatalogService.Dto.Catalog
{
    public class CatalogCategoryResponse
    {

        public string Name { get; set; } = string.Empty;

        public string SystemName { get; set; } = string.Empty;

        public List<CatalogCategoryResponse> ChildrenCategories { get; set; } = [];

        public CatalogCategoryResponse() { }
    }
}
