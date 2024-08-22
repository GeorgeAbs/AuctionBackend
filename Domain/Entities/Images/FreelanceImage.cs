using Domain.Entities.Items.ItemTrading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Images
{
    public class FreelanceImage<T> : EntityImageBase<T> where T : class
    {
        public FreelanceImage(T ownerEntity, string bigImagePath, string smallImagePath)
            : base(ownerEntity, bigImagePath, smallImagePath) { }
    }
}
