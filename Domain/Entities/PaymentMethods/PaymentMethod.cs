using Domain.CoreEnums;

namespace Domain.Entities.PaymentMethods
{
    public class PaymentMethod : EntityBase
    {
        public Enums.PaymentMethod PaymentType { get; private set; }

        private PaymentMethod() { }

        public PaymentMethod(Enums.PaymentMethod paymentType)
        {
            PaymentType = paymentType;
        }
    }
}
