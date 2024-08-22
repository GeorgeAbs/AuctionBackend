using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Domain.CoreEnums.Enums;

namespace Domain.Entities.AuctionSlots
{
    public abstract class BaseAuctionSlot<TBid, TStatusHistory> : EntityBase, IAuctionSlot<TBid> where TBid : class where TStatusHistory : class
    {
        public Guid ItemId { get; private set; }

        public string Title { get; private set; }

        public string Description { get; protected set; }

        public Guid UserId { get; private set; }

        public Guid CustomerId { get; protected set; }

        public float StartPrice { get; protected set; }

        public float Price { get; protected set; }

        public float MinimumBid { get; protected set; }

        protected readonly List<TBid> _bids = new();

        [BackingField(nameof(_bids))]
        public IEnumerable<TBid> Bids { get { return _bids; } }

        public float BlitzPrice { get; protected set; }

        public DateTime AuctionEndingTime { get; protected set; }

        public DateTime PendingOrderFormingStartTime { get; protected set; }

        public int AuctionSlotNum { get; protected set; } = 1;

        /// <summary>
        /// default status is Created
        /// </summary>
        public AuctionSlotStatus Status { get; protected set; } = AuctionSlotStatus.Created;

        protected readonly List<TStatusHistory> _statusHistories = new();
        [BackingField(nameof(_statusHistories))]
        public IEnumerable<TStatusHistory> StatusHistories { get { return _statusHistories; } }

        public BaseAuctionSlot() { }

        public BaseAuctionSlot(Guid itemId, string title, string description, float price, float minimumBid, float blitzPrice,DateTime auctionEndingTime,int auctionSlotNum)
        {
            ItemId = itemId;
            Title = title;
            Description = description;
            StartPrice = Price = price;
            MinimumBid = minimumBid;
            BlitzPrice = blitzPrice;
            AuctionEndingTime = auctionEndingTime;
            AuctionSlotNum = auctionSlotNum;
        }

        public void AddBid(TBid bid)
        {
            _bids.Add(bid);
        }

        public void SetPrice(float price)
        {
            if (price < MinimumBid) return;

            if (price >= BlitzPrice) { Price = BlitzPrice; return; }

            if (price <= Price) { return; }

            Price = price;
        }

        public void SetCustomerId(Guid customerId)
        {
            CustomerId = customerId;
        }

        public abstract void ChangeStatus(AuctionSlotStatus status);

        public void SetPendingOrderFormingStartTime(DateTime time)
        {
            PendingOrderFormingStartTime = time;
        }

        public virtual void PrepareForDeleting()
        {
            _bids.Clear();
        }

    }
}
