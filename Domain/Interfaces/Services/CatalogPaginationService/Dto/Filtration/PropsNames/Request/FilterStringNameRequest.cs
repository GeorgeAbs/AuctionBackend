namespace Domain.Interfaces.Services.CatalogPaginationService.Dto.Filtration.PropsNames.Request
{
    public class FilterStringNameRequest
    {
        public string CatalogNameSystemName { get; }

        public IEnumerable<string> PropsSystemValues { get; }

        public FilterStringNameRequest(string catalogNameSystemName, IEnumerable<string> propsSystemValues)
        {
            CatalogNameSystemName = catalogNameSystemName;
            PropsSystemValues = propsSystemValues;
        }
    }
}
