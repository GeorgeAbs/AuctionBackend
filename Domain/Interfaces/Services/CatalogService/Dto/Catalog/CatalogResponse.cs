namespace Domain.Interfaces.Services.CatalogService.Dto.Catalog
{
    public class CatalogResponse
    {
        public IEnumerable<CatalogCategoryResponse> RootCatalogCategories { get; set; }

        public CatalogResponse(IEnumerable<CatalogCategoryResponse> categories)
        {
            RootCatalogCategories = categories;
        }
    }
}
