using Domain.Interfaces.Services.CatalogService.Dto.CatalogFilter.PropsEditing;

namespace Domain.Interfaces.Services.CatalogService.Dto.CatalogFilter.PropsNamesEditing
{
    public class EditingStringPropNameResponse : EditingBasePropNameResponse
    {
        public IEnumerable<EditingStringProp> Properties { get; set; }

        public EditingStringPropNameResponse(Guid catalogPropertyNameId, string name, IEnumerable<EditingStringProp> properties) : base(catalogPropertyNameId, name)
        {
            Properties = properties;
        }
    }
}
