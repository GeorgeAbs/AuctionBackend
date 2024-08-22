namespace Domain.Interfaces.Services.MyProfileService.Dto.Catalog
{
    public class MyProfileCatalogStringPropertyName
    {
        public string PropertyNameSystemName { get; set; }

        public string? PropertyNameValue { get; set; }

        public List<MyProfileCatalogStringProperty> StringProperties { get; set; }

        public MyProfileCatalogStringPropertyName(string propertyNameSystemName, string? propertyNameValue, List<MyProfileCatalogStringProperty> stringProperties)
        {
            PropertyNameSystemName = propertyNameSystemName;
            PropertyNameValue = propertyNameValue;
            StringProperties = stringProperties;
        }
    }
}
