using Domain.Interfaces.Services.CatalogService.Dto.CatalogFilter.PropsNamesEditing;

namespace Domain.Interfaces.Services.CatalogService.Dto.Catalog
{
    public class CatalogCategoryFullFilter
    {
        public Guid CatalogCategoryId { get;}

        public IEnumerable<EditingStringPropNameResponse> StringPropNames { get;}

        public IEnumerable<EditingBoolPropNameResponse> BoolPropNames { get; }

        public IEnumerable<EditingFloatPropNameResponse> FloatPropNames { get; }

        public IEnumerable<EditingIntPropNameResponse> IntPropNames { get; }

        public CatalogCategoryFullFilter(Guid catalogCategoryId,
            IEnumerable<EditingStringPropNameResponse>? stringPropNames,
            IEnumerable<EditingBoolPropNameResponse>? boolPropNames,
            IEnumerable<EditingFloatPropNameResponse>? floatPropNames,
            IEnumerable<EditingIntPropNameResponse>? intPropNames)
        {
            CatalogCategoryId = catalogCategoryId;
            StringPropNames = stringPropNames ?? [];
            BoolPropNames = boolPropNames ?? [];
            FloatPropNames = floatPropNames ?? [];
            IntPropNames = intPropNames ?? [];
        }
    }
}
