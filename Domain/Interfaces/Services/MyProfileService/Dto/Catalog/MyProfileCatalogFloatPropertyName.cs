using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services.MyProfileService.Dto.Catalog
{
    public class MyProfileCatalogFloatPropertyName
    {
        public string PropertyNameSystemName { get; set; }

        public string? PropertyNameValue { get; set; }

        public MyProfileCatalogFloatPropertyName(string propertyNameSystemName, string? propertyNameValue)
        {
            PropertyNameSystemName = propertyNameSystemName;
            PropertyNameValue = propertyNameValue;
        }
    }
}
