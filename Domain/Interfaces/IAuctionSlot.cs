namespace Domain.Interfaces
{
    public interface IAuctionSlot<TBid> where TBid : class
    {
        public Guid ItemId { get; }

        public Guid CustomerId { get;}

        public float Price { get; }

        public float MinimumBid { get;}

        public IEnumerable<TBid> Bids { get; }

        public float BlitzPrice { get; }

        public DateTime AuctionEndingTime { get; }

        public string Description { get; }
    }
}
