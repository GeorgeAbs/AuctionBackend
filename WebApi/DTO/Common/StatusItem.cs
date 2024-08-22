using Domain.CoreEnums;

namespace WebApi.DTO.Common
{
    public class StatusItem
    {
        public string StringRepresentation { get; private set; }

        public Enums.ItemTradingStatus Status { get; private set; }

        public StatusItem(Enums.ItemTradingStatus status, string stringValue)
        {
            Status = status;
            StringRepresentation = stringValue;
        }
    }
}
