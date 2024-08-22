using Domain.CoreEnums;

namespace WebApi.DTO.Common
{
    public class ReviewCommentsTypeItem
    {
        public string StringRepresentation { get; private set; }

        public Enums.ReviewCommentsTypes SystemValue { get; private set; }

        public ReviewCommentsTypeItem(Enums.ReviewCommentsTypes systemValue, string stringValue)
        {
            StringRepresentation = stringValue;
            SystemValue = systemValue;
        }
    }
}
