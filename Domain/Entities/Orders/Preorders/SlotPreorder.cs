using Domain.Entities.Addresses;

namespace Domain.Entities.Orders.Preorders
{
    public class SlotPreorder : PreorderBase
    {
        public SlotPreorderAddress SlotPreorderAddress { get; private set; }

        public SlotPreorder(Guid customerId, Guid sellerId, Guid itemId, float itemPrice, SlotPreorderAddress slotPreorderAddress)
            : base(customerId, sellerId, itemId, itemPrice)
        {
            SlotPreorderAddress = slotPreorderAddress;
        }
    }
}
