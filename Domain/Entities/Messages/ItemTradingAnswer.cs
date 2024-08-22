using Domain.Entities.Items.ItemTrading;

namespace Domain.Entities.Messages
{
    public class ItemTradingAnswer : MessageBase<ItemTrading>
    {
        public Guid ItemTradingQuestionId { get; private set; }
        public ItemTradingQuestion Question { get; private set; }

        private ItemTradingAnswer() { }

        public ItemTradingAnswer(ItemTrading item, Guid writerId, string text, ItemTradingQuestion question) : base(item, writerId, text)
        {
            Question = question;
        }
    }
}
