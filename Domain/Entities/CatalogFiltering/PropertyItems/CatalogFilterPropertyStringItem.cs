using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.CatalogFiltering.PropertyItems
{
    public class CatalogFilterPropertyStringItem
    {
        public Guid CatalogItemPropertyId { get; set; } // айди варианта характеристики

        public string Value { get; set; } = string.Empty; // отоборажаемое значение

        public bool IsSelected { get; set; } = false;

        public bool IsEnabled { get; set; } = true;

        public CatalogFilterPropertyStringItem() { }
    }
}
