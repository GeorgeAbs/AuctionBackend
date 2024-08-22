namespace Domain.Interfaces.Services.MyProfileService.Dto.Catalog
{
    public class MyProfileCatalogStringProperty
    {
        public string SystemValue { get; set; }

        public string Value { get; set; }

        public MyProfileCatalogStringProperty(string systemValue, string value)
        {
            SystemValue = systemValue;
            Value = value;
        }
    }
}
