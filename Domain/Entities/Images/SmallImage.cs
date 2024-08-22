using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Images
{
    public class SmallImage<T> : EntityBase where T : class
    {
        public string SmallImagePath { get; }

        public T OwnerEntity { get; set; }

        private SmallImage() { }

        public SmallImage(T ownerEntity, string smallImagePath)
        {
            SmallImagePath = smallImagePath;
            OwnerEntity = ownerEntity;
        }
    }
}
