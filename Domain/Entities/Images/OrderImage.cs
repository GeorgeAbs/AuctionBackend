using Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Images
{
    public class OrderImage : SmallImage<ItemTradingOrder>
    {
        public OrderImage(ItemTradingOrder ownerEntity, string smallImagePath) : base(ownerEntity, smallImagePath)
        {

        }
    }
}
