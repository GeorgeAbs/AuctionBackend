using Domain.Interfaces;

namespace Domain.Entities.ModerationDisappReason
{
    public class ModerationDisappReason<TItem> : EntityBase where TItem : IItem
    {
        public TItem Item { get; private set; }

        public string Reason { get; private set; }

        public Guid ModeratorId { get; private set; }

        public ModerationDisappReason() { }

        public ModerationDisappReason(TItem item, string reason, Guid moderatorId)
        {
            Item = item;
            Reason = reason;
            ModeratorId = moderatorId;
        }
    }
}
