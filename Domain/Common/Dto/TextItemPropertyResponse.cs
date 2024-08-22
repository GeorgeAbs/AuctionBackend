namespace Domain.Common.Dto
{
    public class TextItemPropertyResponse
    {
        public string PropertySystemValue { get; }

        public string Value { get; }

        public string PropertyNameSystemName { get; }

        public TextItemPropertyResponse(string propertySystemValue, string value, string propertyNameSystemName)
        {
            PropertySystemValue = propertySystemValue;
            Value = value;
            PropertyNameSystemName = propertyNameSystemName;
        }
    }
}
