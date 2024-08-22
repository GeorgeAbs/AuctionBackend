namespace Domain.Interfaces.Services.MyProfileService.Dto.Catalog
{
    public class MyProfileCatalogBoolProperty
    {
        public string SystemValue { get; set; }

        public string Value { get; set; }

        public MyProfileCatalogBoolProperty(string systemValue, string value)
        {
            SystemValue = systemValue;
            Value = value;
        }
    }
}
