namespace Domain.Interfaces.Services.CatalogService.Dto.Catalog
{
    public class CatalogCategoryCreationRequest
    {
        public string Name { get; private set; }

        public string SystemName {  get; private set; }

        public string ParentCatalogCategorySystemName { get; private set; }

        public CatalogCategoryCreationRequest(string name,string systemName, string parentCatalogCategorySystemName)
        {
            Name = name;
            SystemName = systemName;
            ParentCatalogCategorySystemName = parentCatalogCategorySystemName;
        }
    }
}
