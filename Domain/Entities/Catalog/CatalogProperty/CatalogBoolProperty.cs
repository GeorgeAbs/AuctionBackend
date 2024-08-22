using Domain.Entities.Catalog.CatalogProperty.CatalogPropertiesNames;
using Domain.Entities.Items.ItemTrading;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.Catalog.CatalogProperty
{
    [Index(nameof(SystemValue), IsUnique = true)]
    public class CatalogBoolProperty : CatalogProperty<string, CatalogBoolPropertyName>
    {
        private readonly List<ItemTrading> _items = [];
        [BackingField(nameof(_items))]
        public IEnumerable<ItemTrading> Items { get { return _items; } }

        public string SystemValue {  get; private set; }

        private CatalogBoolProperty() { }


        public CatalogBoolProperty(string propertyValue, CatalogBoolPropertyName propertyName, string systemValue)
            : base(propertyValue, propertyName)
        {
            SystemValue = systemValue.ToLower().Replace(' ', '_').Replace('-', '_');
            propertyName.AddProperty(this);
        }

        public void AddItem(ItemTrading item)
        {
            if(item is not null)
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
                if(_items.Count == 0)
                {
                    IsInUsing = false;
                }
            }
                
        }


    }
}
