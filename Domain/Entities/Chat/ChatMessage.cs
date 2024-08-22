using Domain.Interfaces;

namespace Domain.Entities.Chat
{
    public class ChatMessage : EntityBase
    {
        public Chat ThisChat { get; set; }

        public IUser MessageOwner { get; set; }

        public string MessageBody { get; set; }

        public ChatMessage? ParentMessage { get; set; }

        public List<ChatMessage> ChildrenMessages { get; set; } = new();

        public List<Attachment> Attachments { get; set; } = new();

        public ChatMessage(Chat thisChat, string messageBody, IUser messageOwner)
        {
            ThisChat = thisChat;
            MessageBody = messageBody;
            MessageOwner = messageOwner;
        }
    }
}
