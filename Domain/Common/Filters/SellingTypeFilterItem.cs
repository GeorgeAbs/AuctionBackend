using Domain.CoreEnums;

namespace Domain.Common.Filters
{
    public class SellingTypeFilterItem
    {
        public string StringRepresentation { get; private set; }

        public Enums.SellingTypes SystemValue { get; private set; }

        public SellingTypeFilterItem(Enums.SellingTypes systemValue, string stringRepresentation)
        {
            StringRepresentation = stringRepresentation;
            SystemValue = systemValue;
        }
    }
}
