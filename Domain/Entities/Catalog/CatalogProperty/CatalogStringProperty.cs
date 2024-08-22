using Domain.Entities.Catalog.CatalogProperty.CatalogPropertiesNames;
using Domain.Entities.Items.ItemTrading;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.Catalog.CatalogProperty
{
    public class CatalogStringProperty : CatalogProperty<string, CatalogStringPropertyName>
    {
        private readonly List<ItemTrading> _items = [];
        [BackingField(nameof(_items))]
        public IEnumerable<ItemTrading> Items { get { return _items; } }

        public string SystemValue { get; private set; }

        private CatalogStringProperty() { }

        public CatalogStringProperty(string propertyValue, CatalogStringPropertyName propertyName, string systemValue)
            : base(propertyValue, propertyName)
        {
            SystemValue = systemValue.ToLower().Replace(' ', '_').Replace('-', '_');
            propertyName.AddProperty(this);
        }

        public void AddItem(ItemTrading item)
        {
            if (item is not null)
            {
                _items.Add(item);
                IsInUsing = true;
            }
        }

        public void RemoveItem(ItemTrading item)
        {
            if (item is not null)
            {
                _items.Remove(item);
                if (_items.Count == 0)
                {
                    IsInUsing = false;
                }
            }

        }
    }

    
}
