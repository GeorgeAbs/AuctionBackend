namespace Domain.Interfaces.Services.MyProfileService.Dto.Catalog
{
    public class MyProfileCatalogBoolPropertyName
    {
        public string PropertyNameSystemName { get; set; }

        public string? PropertyNameValue { get; set; }

        public List<MyProfileCatalogBoolProperty> BoolProperties { get; set; } = new();

        public MyProfileCatalogBoolPropertyName(string propertyNameSystemName, string? propertyNameValue, List<MyProfileCatalogBoolProperty> boolProperties)
        {
            PropertyNameSystemName = propertyNameSystemName;
            PropertyNameValue = propertyNameValue;
            BoolProperties = boolProperties;
        }
    }
}
