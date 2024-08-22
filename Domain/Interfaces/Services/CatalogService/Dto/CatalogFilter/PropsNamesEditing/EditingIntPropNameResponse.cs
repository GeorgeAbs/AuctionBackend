using Domain.Interfaces.Services.CatalogPaginationService.Dto.Filtration.Props.Responce;

namespace Domain.Interfaces.Services.CatalogService.Dto.CatalogFilter.PropsNamesEditing
{
    public class EditingIntPropNameResponse : EditingBasePropNameResponse
    {

        public EditingIntPropNameResponse(Guid catalogPropertyNameId, string name) : base(catalogPropertyNameId, name)
        {
        }
    }
}
