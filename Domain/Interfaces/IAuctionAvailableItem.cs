using Domain.Entities.Bid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAuctionAvailableItem
    {
        public int Price { get; }

        public int MinimumBid { get;}

        public int BlitzPrice { get;}

        public DateTime AuctionEndingTime { get;}
    }
}
