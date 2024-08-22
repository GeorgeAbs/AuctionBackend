namespace Domain.Entities.Catalog.CatalogProperty.CatalogPropertiesNames
{
    public class CatalogFloatPropertyName : CatalogPropertyNameBase<CatalogFloatProperty>
    {
        private CatalogFloatPropertyName() { }
        public CatalogFloatPropertyName(CatalogCategory catalogCategory, string name, string systemName) : base(catalogCategory, name, systemName) { }

    }
}
