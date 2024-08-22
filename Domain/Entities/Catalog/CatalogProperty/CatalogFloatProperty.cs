using Domain.Entities.Catalog.CatalogProperty.CatalogPropertiesNames;
using Domain.Entities.Items.ItemTrading;

namespace Domain.Entities.Catalog.CatalogProperty
{
    public class CatalogFloatProperty : CatalogProperty<float, CatalogFloatPropertyName>
    {
        public ItemTrading Item {  get; private set; }

        private CatalogFloatProperty() { }

        public CatalogFloatProperty(float propertyValue, ItemTrading itemAsOwner, CatalogFloatPropertyName propertyName)
            : base(propertyValue, propertyName) 
        {
            Item = itemAsOwner;
            propertyName.AddProperty(this);
        }
    }
}
