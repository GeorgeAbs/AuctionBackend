using Domain.Entities.Catalog.CatalogProperty.CatalogPropertiesNames;
using Domain.Entities.Items.ItemTrading;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.Catalog
{
    public class CatalogCategory : EntityBase
    {
        /// <summary>
        /// displayed value in UI
        /// </summary>
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// name used for fitration. Must be unique
        /// </summary>
        public string SystemName { get; private set; } = string.Empty;

        public CatalogCategory? ParentCatalogCategory { get; private set; }

        private readonly List<CatalogCategory> _childrenCatalogCategories = new();
        [BackingField(nameof(_childrenCatalogCategories))]
        public IEnumerable<CatalogCategory> ChildrenCatalogCategories { get { return _childrenCatalogCategories; } }

        private readonly List<ItemTrading> _itemsTrading = new();
        [BackingField(nameof(_itemsTrading))]
        public IEnumerable<ItemTrading> ItemsTrading { get { return _itemsTrading; } }

        private readonly List<CatalogFloatPropertyName> _catalogFloatPropertyNames = new();
        [BackingField(nameof(_catalogFloatPropertyNames))]
        public IEnumerable<CatalogFloatPropertyName> CatalogFloatPropertyNames { get { return _catalogFloatPropertyNames; } }

        private readonly List<CatalogIntPropertyName> _catalogIntPropertyNames = new();
        [BackingField(nameof(_catalogIntPropertyNames))]
        public IEnumerable<CatalogIntPropertyName> CatalogIntPropertyNames { get { return _catalogIntPropertyNames; } }

        private readonly List<CatalogStringPropertyName> _catalogStringPropertyNames = new();
        [BackingField(nameof(_catalogStringPropertyNames))]
        public IEnumerable<CatalogStringPropertyName> CatalogStringPropertyNames { get { return _catalogStringPropertyNames; } }

        private readonly List<CatalogBoolPropertyName> _catalogBoolPropertyNames = new();
        [BackingField(nameof(_catalogBoolPropertyNames))]
        public IEnumerable<CatalogBoolPropertyName> CatalogBoolPropertyNames { get { return _catalogBoolPropertyNames; } }

        public bool IsVisible { get; private set; } = true;

        private CatalogCategory() { }

        public CatalogCategory(string name, string systemName, CatalogCategory? parentCatalogCategory = null)
        {
            Name = name;
            SystemName = systemName.ToLower().Replace(' ', '_').Replace('-', '_');
            ParentCatalogCategory = parentCatalogCategory;
        }

        public void ChangeVisibility(bool isVisible)
        {
            IsVisible = isVisible;
        }

        public void SetName(string newName)
        {
            if (newName is not null)
                Name = newName;
        }

        public void SetParentCategory(CatalogCategory parentrenCategory)
        {
            if (parentrenCategory is not null)
                ParentCatalogCategory = parentrenCategory;
        }

        public void AddChildrenCategory(CatalogCategory childrenCategory)
        {
            if(childrenCategory is not null) 
                _childrenCatalogCategories.Add(childrenCategory);
        }

        public void RemoveChildrenCategory(CatalogCategory childrenCategory)
        {
            if (childrenCategory is not null)
                _childrenCatalogCategories.Remove(childrenCategory);
        }

        public void RemoveAllChildrenCategories()
        {
            _childrenCatalogCategories.Clear();
        }

        public void AddItem(ItemTrading item)
        {
            if (item is not null)
                _itemsTrading.Add(item);
        }

        public void RemoveItem(ItemTrading item)
        {
            if (item is not null)
                _itemsTrading.Remove(item);
        }

        public void RemoveAllItems()
        {
            _itemsTrading.Clear();
        }

        public void AddCatalogPropertyName(CatalogFloatPropertyName propertyName)
        {
            if (propertyName is not null)
                _catalogFloatPropertyNames.Add(propertyName);
        }

        public void RemoveCatalogPropertyName(CatalogFloatPropertyName propertyName)
        {
            if (propertyName is not null)
                _catalogFloatPropertyNames.Remove(propertyName);
        }

        public void RemoveAllCatalogFloatPropertyNames()
        {
            _catalogFloatPropertyNames.Clear();
        }

        public void AddCatalogPropertyName(CatalogIntPropertyName propertyName)
        {
            if (propertyName is not null)
                _catalogIntPropertyNames.Add(propertyName);
        }

        public void RemoveCatalogPropertyName(CatalogIntPropertyName propertyName)
        {
            if (propertyName is not null)
                _catalogIntPropertyNames.Remove(propertyName);
        }

        public void RemoveAllCatalogIntPropertyNames()
        {
            _catalogIntPropertyNames.Clear();
        }

        public void AddCatalogPropertyName(CatalogStringPropertyName propertyName)
        {
            if (propertyName is not null)
                _catalogStringPropertyNames.Add(propertyName);
        }

        public void RemoveCatalogPropertyName(CatalogStringPropertyName propertyName)
        {
            if (propertyName is not null)
                _catalogStringPropertyNames.Remove(propertyName);
        }

        public void RemoveAllCatalogStringPropertyNames()
        {
            _catalogStringPropertyNames.Clear();
        }

        public void AddCatalogPropertyName(CatalogBoolPropertyName propertyName)
        {
            if (propertyName is not null)
                _catalogBoolPropertyNames.Add(propertyName);
        }

        public void RemoveCatalogPropertyName(CatalogBoolPropertyName propertyName)
        {
            if (propertyName is not null)
                _catalogBoolPropertyNames.Remove(propertyName);
        }

        public void RemoveAllCatalogBoolPropertyNames()
        {
            _catalogBoolPropertyNames.Clear();
        }

    }
}
