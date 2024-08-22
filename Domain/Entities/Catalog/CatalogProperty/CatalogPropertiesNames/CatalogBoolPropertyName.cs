namespace Domain.Entities.Catalog.CatalogProperty.CatalogPropertiesNames
{
    public class CatalogBoolPropertyName : CatalogPropertyNameBase<CatalogBoolProperty>
    {
        private CatalogBoolPropertyName() { }
        public CatalogBoolPropertyName(CatalogCategory catalogCategory, string name, string systemName) : base(catalogCategory, name,systemName) { }

    }
}
