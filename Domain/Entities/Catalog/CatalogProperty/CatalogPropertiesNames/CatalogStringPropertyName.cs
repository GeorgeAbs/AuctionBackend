namespace Domain.Entities.Catalog.CatalogProperty.CatalogPropertiesNames
{
    public class CatalogStringPropertyName : CatalogPropertyNameBase<CatalogStringProperty>
    {
        private CatalogStringPropertyName() { }
        public CatalogStringPropertyName(CatalogCategory catalogCategory, string name, string systemName) : base(catalogCategory, name, systemName) { }

    }
}
