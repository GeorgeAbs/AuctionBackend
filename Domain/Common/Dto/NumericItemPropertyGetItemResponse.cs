namespace Domain.Common.Dto
{
    public class NumericItemPropertyGetItemResponse<TValue>
    {
        public TValue Value { get; }

        public string PropertyName { get; }

        public NumericItemPropertyGetItemResponse(TValue value, string propertyName)
        {
            Value = value;
            PropertyName = propertyName;
        }
    }
}
