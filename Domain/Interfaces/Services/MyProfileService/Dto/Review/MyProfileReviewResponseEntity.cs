namespace Domain.Interfaces.Services.MyProfileService.Dto.Review
{
    public class MyProfileReviewResponseEntity
    {
        public Guid ReviewId { get; private set; }

        public Guid WriterId { get; private set; }

        public string ReviewWriterName { get; private set; } = string.Empty;

        public string Body { get; private set; } = string.Empty;

        public DateTime CreationTime { get; private set; }

        public bool IsOwner { get; private set; }

        public MyProfileReviewResponseEntity(Guid reviewId,
            Guid writerId,
            string writerUserName,
            string body,
            DateTime creationTime,
            Guid? currentUserId)
        {
            ReviewId = reviewId;
            WriterId = writerId;
            ReviewWriterName = writerUserName;
            Body = body;
            CreationTime = creationTime;
            IsOwner = currentUserId == writerId;
        }
    }
}
