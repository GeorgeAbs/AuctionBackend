using Domain.Entities.Items;

namespace Domain.Entities.Messages
{
    public abstract class MessageBase<TMessageOwner> : EntityBase where TMessageOwner : class
    {
        /// <summary>
        /// Item or User or whatever (for example, question for item or review for item)
        /// </summary>
        public TMessageOwner MessageOwner { get; private set; }
        /// <summary>
        /// UserId of whom wrote message
        /// </summary>
        public Guid WriterId { get; private set; }

        public string Text { get; private set; }

        public MessageBase() { }

        public MessageBase(TMessageOwner messageOwner, Guid writerId, string text)
        {
            MessageOwner = messageOwner;
            WriterId = writerId;
            Text = text;
        }
    }
}
