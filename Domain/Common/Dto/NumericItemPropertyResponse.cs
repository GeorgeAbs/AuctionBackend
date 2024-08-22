namespace Domain.Common.Dto
{
    public class NumericItemPropertyResponse<TValue>
    {
        public TValue Value { get; }

        public string PropertyNameSystemName { get; }

        public NumericItemPropertyResponse(TValue value, string propertyNameSystemName)
        {
            Value = value;
            PropertyNameSystemName = propertyNameSystemName;
        }
    }
}
