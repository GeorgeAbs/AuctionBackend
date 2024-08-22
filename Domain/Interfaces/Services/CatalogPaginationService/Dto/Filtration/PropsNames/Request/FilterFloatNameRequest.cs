namespace Domain.Interfaces.Services.CatalogPaginationService.Dto.Filtration.PropsNames.Request
{
    public class FilterFloatNameRequest
    {
        public string CatalogNameSystemName { get; }

        public float MinValue { get; }

        public float MaxValue { get; }

        public FilterFloatNameRequest(string catalogNameSystemName, float mnValue, float maxValue)
        {
            CatalogNameSystemName = catalogNameSystemName;
            MinValue = mnValue;
            MaxValue = maxValue;
        }
    }
}
