using Domain.Entities.CatalogFiltering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.CatalogFiltering.PropertyItems
{
    public class CatalogFilterStringProperties : CatalogFilterProperties
    {
        // есть айди свойства и имя свойства (Цвет и айди, Материал и айди)
        public List<CatalogFilterPropertyStringItem> PropertyItems { get; set; } = new();

        public CatalogFilterStringProperties() { }

    }
}
