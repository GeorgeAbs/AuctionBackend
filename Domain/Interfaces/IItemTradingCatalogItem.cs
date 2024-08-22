using Domain.Entities.Catalog.CatalogProperty;
using Domain.Entities.Catalog;

namespace Domain.Interfaces
{
    public interface IItemTradingCatalogItem
    {
        public CatalogCategory CatalogCategory { get; }

        public List<CatalogStringProperty> StringProperties { get; }

        public List<CatalogFloatProperty> FloatProperties { get; }

        public List<CatalogIntProperty> IntProperties { get; }

        public List<CatalogBoolProperty> BoolProperties { get; }
    }
}
