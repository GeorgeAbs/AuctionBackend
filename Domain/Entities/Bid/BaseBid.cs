using Domain.Interfaces;

namespace Domain.Entities.Bid
{
    public class BaseBid<TSlot> : EntityBase, IBid<TSlot> where TSlot : class
    {
        public Guid BidOwnerId { get; set; }

        public float BidAmount { get; set; }

        public TSlot BidSlot { get; set; }

        public BaseBid() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bidOwnerId"></param>
        /// <param name="bidAmount"></param>
        /// <param name="bidSlot"></param>
        public BaseBid(Guid bidOwnerId, float bidAmount, TSlot bidSlot)
        {
            BidOwnerId = bidOwnerId;
            BidAmount = bidAmount;
            BidSlot = bidSlot;
        }
    }
}
