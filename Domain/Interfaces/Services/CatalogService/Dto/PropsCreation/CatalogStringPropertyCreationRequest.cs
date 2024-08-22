using System.ComponentModel.DataAnnotations;

namespace Domain.Interfaces.Services.CatalogService.Dto.PropsCreation
{
    public class CatalogStringPropertyCreationRequest
    {
        [Required]
        public Guid CatalogStringPropNameId { get; }

        [Required]
        public string Value { get; }

        [Required]
        public string SystemValue { get; }

        public CatalogStringPropertyCreationRequest(Guid catalogStringPropNameId, string value,  string systemValue) 
        {
            CatalogStringPropNameId = catalogStringPropNameId;
            Value = value;
            SystemValue = systemValue;
        }
    }
}
