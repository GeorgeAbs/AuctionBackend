using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.CatalogFiltering.PropertyItems
{
    public class CatalogFilterPropertyIntItem
    {
        public Guid CatalogItemPropertyId { get; set; }

        public int Value { get; set; }

        public bool IsSelected { get; set; } = false;

        public bool IsEnabled { get; set; } = true;

        public CatalogFilterPropertyIntItem() { }
    }
}
