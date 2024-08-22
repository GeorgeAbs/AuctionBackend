namespace Domain.Interfaces.Services.CatalogService.Dto.CatalogFilter.PropsEditing
{
    public class EditingStringProp
    {
        public Guid PropertyId { get; }

        public string Value { get; }
        public EditingStringProp(Guid propertyId, string value)
        {
            PropertyId = propertyId;
            Value = value;
        }
    }
}
