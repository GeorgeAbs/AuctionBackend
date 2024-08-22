namespace Domain.Interfaces.Services.MyProfileService.Dto.Comment
{
    public class MyProfileCommentResponseEntity
    {
        public Guid CommentId { get; private set; }

        public Guid WriterId { get; private set; }

        public string WriterUserName { get; private set; }

        public string Body { get; private set; }

        public DateTime CreationTime { get; private set; }

        public bool IsOwner { get; private set; }

        public MyProfileCommentResponseEntity(Guid commentId,
            Guid commentWriterId,
            string writerName,
            string body,
            DateTime creationTime,
            Guid? currentUserId)
        {
            CommentId = commentId;
            WriterId = commentWriterId;
            WriterUserName = writerName;
            Body = body;
            CreationTime = creationTime;
            IsOwner = currentUserId == commentWriterId;
        }
    }
}
