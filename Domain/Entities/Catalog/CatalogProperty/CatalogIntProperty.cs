using Domain.Entities.Catalog.CatalogProperty.CatalogPropertiesNames;
using Domain.Entities.Items.ItemTrading;

namespace Domain.Entities.Catalog.CatalogProperty
{
    public class CatalogIntProperty : CatalogProperty<int,  CatalogIntPropertyName>
    {
        public ItemTrading Item { get; private set; }

        private CatalogIntProperty() { }

        public CatalogIntProperty(int propertyValue, ItemTrading itemAsOwner, CatalogIntPropertyName propertyName)
            : base(propertyValue, propertyName) 
        { 
            Item = itemAsOwner;
            propertyName.AddProperty(this);
        }
    }
}
