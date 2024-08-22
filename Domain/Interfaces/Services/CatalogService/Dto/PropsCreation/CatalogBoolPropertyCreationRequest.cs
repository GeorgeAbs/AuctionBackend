using System.ComponentModel.DataAnnotations;

namespace Domain.Interfaces.Services.CatalogService.Dto.PropsCreation
{
    public class CatalogBoolPropertyCreationRequest
    {
        [Required]
        public Guid CatalogStringPropNameId { get; }

        [Required]
        public string TrueValue { get; }

        [Required]
        public string FalseValue { get; }

        [Required]
        public string SystemTrueValue { get; }

        [Required]
        public string SystemFalseValue { get; }

        public CatalogBoolPropertyCreationRequest(Guid catalogStringPropNameId, string trueValue, string falseValue, string systemTrueValue, string systemFalseValue)
        {
            CatalogStringPropNameId = catalogStringPropNameId;
            TrueValue = trueValue;
            FalseValue = falseValue;
            SystemFalseValue = systemFalseValue;
            SystemTrueValue = systemTrueValue;
            SystemFalseValue = systemFalseValue;
        }
    }
}
