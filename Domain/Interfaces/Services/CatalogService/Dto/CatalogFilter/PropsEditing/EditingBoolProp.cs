namespace Domain.Interfaces.Services.CatalogService.Dto.CatalogFilter.PropsEditing
{
    public class EditingBoolProp
    {
        public Guid PropertyId { get; }

        public string Value { get; }
        public EditingBoolProp(Guid propertyId, string value)
        {
            PropertyId = propertyId;
            Value = value;
        }
    }
}
