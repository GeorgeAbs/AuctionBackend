namespace Domain.Interfaces.Services.CatalogPaginationService.Dto.Filtration.Props.Responce
{
    public class FilerTextPropertyBase
    {
        public string SystemValue { get; }

        public string Value { get; }

        public FilerTextPropertyBase(string systemValue, string value)
        {
            SystemValue = systemValue;
            Value = value;
        }
    }
}
