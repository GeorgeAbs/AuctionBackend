namespace Domain.Entities.Catalog.CatalogProperty
{
    public class CatalogProperty<TValue,TPropertyName> : EntityBase where TPropertyName : class
    {
        public TValue PropertyValue { get; private set; } // displaying value (black, wooden)

        public TPropertyName PropertyName { get; private set; }

        public bool IsVisible { get; private set; } = true;

        public bool IsInUsing { get; protected set; } = false;

        public CatalogProperty() { }

        public CatalogProperty(TValue propertyValue, TPropertyName propertyName)
        {
            PropertyValue = propertyValue;
            PropertyName = propertyName;
        }
    }
}
