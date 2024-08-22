using Domain.Entities.Items.ItemTrading;

namespace Domain.Entities.Messages
{
    public class ItemTradingQuestion : MessageBase<ItemTrading>
    {
        public ItemTradingAnswer? Answer { get; private set; }

        private ItemTradingQuestion() { }

        public ItemTradingQuestion(ItemTrading item, Guid writerId, string text) : base(item, writerId, text)
        {

        }

    }
}
