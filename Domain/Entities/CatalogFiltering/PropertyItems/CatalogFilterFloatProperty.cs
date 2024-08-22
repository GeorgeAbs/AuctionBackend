using Domain.Entities.CatalogFiltering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.CatalogFiltering.PropertyItems
{
    public class CatalogFilterFloatProperty : CatalogFilterProperties
    {
        public float MinValue { get; set; } = 0;

        public float MaxValue { get; set; } = 99999999;

        public float MinSelectedValue { get; set; } = 0;

        public float MaxSelectedValue { get; set; } = 99999999;

        public CatalogFilterFloatProperty() { }
    }
}
