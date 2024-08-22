namespace Domain.Interfaces.Services.CatalogPaginationService.Dto.Filtration.PropsNames.Request
{
    public class FilterBoolNameRequest
    {
        public string CatalogNameSystemName { get; }

        public IEnumerable<string> PropsSystemValues { get; }

        public FilterBoolNameRequest(string catalogNameSystemName, IEnumerable<string> propsSystemValues)
        {
            CatalogNameSystemName = catalogNameSystemName;
            PropsSystemValues = propsSystemValues;
        }
    }
}
