namespace Domain.Interfaces.Services.CatalogService.Dto.CatalogFilter.PropsNamesEditing
{
    public class EditingBasePropNameResponse
    {
        public Guid CatalogPropertyNameId { get; }

        public string Name { get; }

        public EditingBasePropNameResponse(Guid catalogPropertyNameId, string name)
        {
            CatalogPropertyNameId = catalogPropertyNameId;
            Name = name;
        }
    }
}
