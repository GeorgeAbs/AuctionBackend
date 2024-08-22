using Domain.Entities.Images;
using Domain.Entities.Items.ItemTrading;

namespace Domain.Entities.Comments
{
    public class Comment : EntityBase
    {
        /// <summary>
        /// User Id
        /// </summary>
        public Guid WriterId { get; private set; }

        public string Body { get; private set; } = "";

        public ItemTrading CommentedItem { get; private set; }

        public IReadOnlyCollection<AttachmentImage<Comment>> Images { get; private set; }

        private Comment() { }

        public Comment(Guid writerId, string body, ItemTrading commentedItem, List<AttachmentImage<Comment>>? images)
        {
            WriterId = writerId;
            Body = body;
            CommentedItem = commentedItem;
            Images = images != null ? images.AsReadOnly() : [];
        }
    }
}
