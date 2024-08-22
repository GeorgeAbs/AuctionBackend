namespace Domain.Interfaces.Services.CatalogPaginationService.Dto.Filtration.PropsNames.Request
{
    public class FilterIntNameRequest
    {
        public string CatalogNameSystemName { get; }

        public int MinValue { get; }

        public int MaxValue { get; }

        public FilterIntNameRequest(string catalogNameSystemName, int mnValue, int maxValue)
        {
            CatalogNameSystemName = catalogNameSystemName;
            MinValue = mnValue;
            MaxValue = maxValue;
        }
    }
}
