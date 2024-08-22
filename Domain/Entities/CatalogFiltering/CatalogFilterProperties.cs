using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.CatalogFiltering
{
    public class CatalogFilterProperties
    {
        public string DisplayedName { get; set; } = string.Empty;// Цвет, Размер, Материал...here CatalogPropertyName

        public Guid CatalogPropertyId { get; set; } // айди типа свойства, например айди свойства Цвет (в цвете есть уже айтемы со своими айди и именами)

        public CatalogFilterProperties() { }
    }
}
