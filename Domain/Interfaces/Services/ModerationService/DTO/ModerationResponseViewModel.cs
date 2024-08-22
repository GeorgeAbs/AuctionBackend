namespace Domain.Interfaces.Services.ModerationService.DTO
{
    public abstract class ModerationResponseViewModel<TItemInfo> where TItemInfo : class
    {
        public TItemInfo ItemInfo { get; set; }
        public List<string> AutoModerationMessages { get; set; }

        protected ModerationResponseViewModel(TItemInfo itemInfo, List<string> autoModerationMessages)
        {
            ItemInfo = itemInfo;
            AutoModerationMessages = autoModerationMessages;
        }
    }
}
