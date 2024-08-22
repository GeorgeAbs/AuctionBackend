using Domain.Entities;

namespace Domain.Interfaces.Services.MyProfileService.Dto
{
    public class MyProfilePaymentMethod : EntityBase
    {
        public string PaymentMethodName { get; }

        public MyProfilePaymentMethod(string paymentMethodName)
        {
            PaymentMethodName = paymentMethodName;
        }
    }
}
