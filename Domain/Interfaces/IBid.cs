using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IBid<TSot> where TSot : class
    {
        public Guid BidOwnerId { get; }

        public float BidAmount { get;}

        public TSot BidSlot { get; }
    }
}
