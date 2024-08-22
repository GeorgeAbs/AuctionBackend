namespace Domain.Interfaces.Services.MyProfileService.Dto.Catalog
{
    public class MyProfileCatalogCategory
    {
        public string SystemName { get; set; } = string.Empty;

        public string DisplayedName { get; set; } = string.Empty;

        public List<MyProfileCatalogCategory> ChildrenCategories { get; set; } = [];

        public List<MyProfileCatalogStringPropertyName> StringPropertyNames { get; set; } = new();

        public List<MyProfileCatalogIntPropertyName> IntPropertyNames { get; set; } = new();

        public List<MyProfileCatalogFloatPropertyName> FloatPropertyNames { get; set; } = new();

        public List<MyProfileCatalogBoolPropertyName> BoolPropertyNames { get; set; } = new();

        public MyProfileCatalogCategory()
        {
        }
    }
}
