using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.Catalog.CatalogProperty.CatalogPropertiesNames
{
    public class CatalogPropertyNameBase<TProperty> : EntityBase where TProperty : class
    {
        public CatalogCategory CatalogCategory { get; private set; }
        /// <summary>
        /// displayed value in UI
        /// </summary>
        public string Name { get; private set; } = string.Empty;// Color, Material...

        /// <summary>
        /// name used for filtration. Must be unique
        /// </summary>
        public string SystemName { get; private set; } = string.Empty;

        /// <summary>
        /// Properties of <TProperty>PropertyName</TProperty>
        /// </summary>
        private readonly List<TProperty> _properties = [];
        [BackingField(nameof(_properties))]
        public IEnumerable<TProperty> Properties { get { return _properties; } }

        /// <summary>
        /// is visible in UI (not required for admin panel)
        /// </summary>
        public bool IsVisible { get; set; } = true;

        public CatalogPropertyNameBase() { }

        public CatalogPropertyNameBase(CatalogCategory catalogCategory, string name, string systemName)
        {
            CatalogCategory = catalogCategory;
            Name = name;
            SystemName = systemName.ToLower().Replace(' ','_').Replace('-','_');
        }

        public void AddProperty(TProperty property)
        {
            if (property is not null)
                _properties.Add(property);
        }

        public void RemoveProperty(TProperty property)
        {
            if (property is not null)
                _properties.Remove(property);
        }

        public void RemoveAllProperties()
        {
            _properties.Clear();
        }

    }
}
