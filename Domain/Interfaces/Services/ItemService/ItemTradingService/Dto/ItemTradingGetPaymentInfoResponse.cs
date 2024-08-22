using Domain.CoreEnums;

namespace Domain.Interfaces.Services.ItemService.ItemTradingService.Dto
{
    public class ItemTradingGetPaymentInfoResponse
    {
        public IEnumerable<string> PaymentMethods { get; private set; } = [];

        public void SetInfo(IEnumerable<string> paymentMethods)
        {
            PaymentMethods = paymentMethods;
        }


    }
}
