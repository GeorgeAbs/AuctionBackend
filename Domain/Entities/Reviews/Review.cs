using Domain.Entities.Comments;
using Domain.Entities.Images;

namespace Domain.Entities.Reviews
{
    public class Review : EntityBase
    {
        /// <summary>
        /// User Id or Item Id which is reviewed, depends what is reviewed
        /// </summary>
        public Guid OwnerId { get; private set; }

        /// <summary>
        /// User id of who wrote review
        /// </summary>
        public Guid WriterId { get; private set; }

        /// <summary>
        /// review body (html/txt/etc...)
        /// </summary>
        public string Body { get; private set; }

        //public IReadOnlyCollection<AttachmentImage<Comment>> Images { get; private set; }

        private Review() { }

        public Review(Guid ownerId, Guid writerId, string body /*List<AttachmentImage<Comment>>? images*/)
        {
            OwnerId = ownerId;
            WriterId = writerId;
            Body = body;
            //Images = images != null ? images.AsReadOnly() : [];
        }
    }
}
