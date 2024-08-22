using Domain.Entities.Messages;

namespace Domain.Entities.Images
{
    public class ItemTradingReviewImage : AttachmentImage<ItemTradingReview>
    {
        private ItemTradingReviewImage() { }

        public ItemTradingReviewImage(ItemTradingReview ownerEntity, string bigImagePath, string smallImagePath) : base(ownerEntity, bigImagePath, smallImagePath)
        {
        }
    }
}
