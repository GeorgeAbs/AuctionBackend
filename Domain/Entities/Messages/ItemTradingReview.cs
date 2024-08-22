using Domain.Entities.Images;
using Domain.Entities.UserEntity;

namespace Domain.Entities.Messages
{
    public class ItemTradingReview : MessageBase<User>
    {
        public Guid ItemId { get; set; }

        public string ItemTitle { get; set; }

        public int Mark {  get; private set; }

        public List<ItemTradingReviewImage> ReviewImages { get; set; }

        public ItemTradingReviewResponse? ReviewResponse { get; private set; }

        public ItemTradingReview(User messageOwner, Guid writerId, string text, Guid itemId, int mark, string itemTitle) : base(messageOwner, writerId, text)
        {
            ItemId = itemId;
            Mark = mark;
            ItemTitle = itemTitle;
        }

        private ItemTradingReview() { }


    }
}
