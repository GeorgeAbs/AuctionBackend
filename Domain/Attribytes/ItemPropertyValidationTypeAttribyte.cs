using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CoreEnums;

namespace Domain.Attribytes
{
    public class ItemPropertyValidationTypeAttribyte : Attribute
    {
        public Enums.ItemPropertyValidationType ItemFieldValidationType { get; set; }
        public ItemPropertyValidationTypeAttribyte(Enums.ItemPropertyValidationType itemFieldValidationType) =>
            ItemFieldValidationType = itemFieldValidationType;


    }
}
