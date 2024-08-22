using Domain.Entities.UserEntity;

namespace Domain.Entities.Messages
{
    public class ItemTradingReviewResponse : MessageBase<User>
    {
        public Guid ItemTradingReviewId { get; private set; }
        public ItemTradingReview Review { get; private set; }

        public ItemTradingReviewResponse(User messageOwner, Guid writerId, string text, ItemTradingReview review) : base(messageOwner, writerId, text)
        {
            Review = review;
        }

        private ItemTradingReviewResponse() { }
    }
}
