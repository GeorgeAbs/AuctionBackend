using Domain.Interfaces;
using static Domain.CoreEnums.Enums;

namespace Domain.Entities.Chat
{
    public class Chat : EntityBase
    {
        public IUser ChatOwner { get; set; }

        public List<ChatMessage> ChatMessages { get; set; } = new();

        public ChatStatuses Status { get; set; } = ChatStatuses.Opened;

        public Chat(IUser chatOwner)
        {
            ChatOwner = chatOwner;
        }
    }
}
