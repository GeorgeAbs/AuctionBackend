using System.ComponentModel.DataAnnotations;
namespace WebApi.ViewModels.Basket
{
    public class AddRemoveBasketItemViewModel
    {
        public Guid ItemTradingId { get; set; }

        [Range(1, 1000000)]
        public int Amount { get; set; } = 1;
    }
}
