namespace Domain.Common.Dto
{
    public class TextItemPropertyRequest
    {
        public string SystemValue { get; private set; }

        public string PropNameSystemName { get; private set; }

        public TextItemPropertyRequest(string systemValue, string propNameSystemName)
        {
            SystemValue = systemValue;
            PropNameSystemName = propNameSystemName;
        }
    }
}
