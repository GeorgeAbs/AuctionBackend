

namespace Domain.Entities.CatalogFiltering
{
    public class CatalogFilterPropertyPriceFilter
    {
        public float MinValue { get; set; } = 0;

        public float MaxValue { get; set; } = 999999999;

        public float MinSelectedValue { get; set; } = 0;

        public float MaxSelectedValue { get; set; } = 999999999;

        public CatalogFilterPropertyPriceFilter() { }
    }
}
