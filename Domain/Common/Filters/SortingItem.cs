using Domain.CoreEnums;

namespace Domain.Common.Filters
{
    public class SortingItem
    {
        public string StringRepresentation { get; private set; }

        public Enums.SortingMethods SystemValue { get; private set; }

        public SortingItem(Enums.SortingMethods systemValue, string stringValue)
        {
            StringRepresentation = stringValue;
            SystemValue = systemValue;
        }
    }
}
