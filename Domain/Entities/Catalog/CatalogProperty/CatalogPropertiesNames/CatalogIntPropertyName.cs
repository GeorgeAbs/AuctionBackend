namespace Domain.Entities.Catalog.CatalogProperty.CatalogPropertiesNames
{
    public class CatalogIntPropertyName : CatalogPropertyNameBase<CatalogIntProperty>
    {
        private CatalogIntPropertyName() { }
        public CatalogIntPropertyName(CatalogCategory catalogCategory, string name, string systemName) : base(catalogCategory, name, systemName) { }

    }
}
