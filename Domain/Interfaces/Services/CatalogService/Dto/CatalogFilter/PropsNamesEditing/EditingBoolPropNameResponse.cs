using Domain.Interfaces.Services.CatalogService.Dto.CatalogFilter.PropsEditing;

namespace Domain.Interfaces.Services.CatalogService.Dto.CatalogFilter.PropsNamesEditing
{
    public class EditingBoolPropNameResponse : EditingBasePropNameResponse
    {
        public IEnumerable<EditingBoolProp> Properties { get; set; }

        public EditingBoolPropNameResponse(Guid catalogPropertyNameId, string name, IEnumerable<EditingBoolProp> properties) : base(catalogPropertyNameId, name)
        {
            Properties = properties;
        }
    }
}
