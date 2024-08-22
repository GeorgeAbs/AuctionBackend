namespace Domain.Interfaces.Services.MyProfileService.Dto.History
{
    public class MyProfileItemTradingHistoryEntity
    {
        public Guid ItemId { get; private set; }

        public string Title { get; private set; }

        public DateTime CurrentStatusChanginTime { get; private set; }

        public string Picture { get; private set; }

        public string Status { get; private set; }


        public MyProfileItemTradingHistoryEntity(Guid itemId, string title, DateTime currentStatusChanginTime, string picture, string status)
        {
            ItemId = itemId;
            Title = title;
            CurrentStatusChanginTime = currentStatusChanginTime;
            Picture = picture;
            Status = status;
        }
    }
}
