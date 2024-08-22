namespace Domain.Common.Dto
{
    public class NumericItemPropertyRequest<TValue>
    {
        public TValue Value { get; private set; }

        public string PropNameSystemName { get; private set; }

        public NumericItemPropertyRequest(TValue value, string propNameSystemName)
        {
            Value = value;
            PropNameSystemName = propNameSystemName;
        }
    }
}
