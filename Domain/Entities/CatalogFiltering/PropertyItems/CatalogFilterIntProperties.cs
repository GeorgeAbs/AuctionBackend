using Domain.Entities.CatalogFiltering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.CatalogFiltering.PropertyItems
{
    public class CatalogFilterIntProperties : CatalogFilterProperties
    {
        public List<CatalogFilterPropertyIntItem> PropertyItems { get; set; } = new();

        public CatalogFilterIntProperties() { }
    }
}
