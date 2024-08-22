using static Domain.CoreEnums.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Interfaces.Services.CatalogService.Dto.Catalog
{
    public class CatalogPropertyNameCreationRequest
    {
        [Required]
        public string CatalogCategorySystemName { get; }

        [Required]
        public CatalogPropertyTypes CatalogPropertyType { get; }

        [Required]
        public string Name { get; }

        [Required]
        public string SystemName { get; }

        public CatalogPropertyNameCreationRequest(string catalogCategorySystemName, CatalogPropertyTypes catalogPropertyType, string name, string systemName)
        {
            CatalogCategorySystemName = catalogCategorySystemName;
            CatalogPropertyType = catalogPropertyType;
            Name = name;
            SystemName = systemName;
        }
    }
}
