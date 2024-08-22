using static Domain.CoreEnums.Enums;
using System.Collections.ObjectModel;
using Domain.CoreEnums;

namespace Domain.Common
{
    public static class Translator
    {
        public static string Translate(DeliveryMethod deliveryMethod)
        {
            return DeliveryMethods[deliveryMethod];
        }

        public static string Translate(PaymentMethod paymentMethod)
        {
            return PaymentMethods[paymentMethod];
        }
        /// <summary>
        /// dictionary of sorting methods (price and date)
        /// </summary>
        private static ReadOnlyDictionary<DeliveryMethod, string> DeliveryMethods { get; } = new(
            new Dictionary<DeliveryMethod, string>()
            {
                { DeliveryMethod.Boxberry, "Boxberry" },
                { DeliveryMethod.PostOfRussia, "Почта России" }
            }
        );

        private static ReadOnlyDictionary<PaymentMethod, string> PaymentMethods { get; } = new(
            new Dictionary<PaymentMethod, string>()
            {
                { PaymentMethod.SafeDeal, "Безопасная сделка" }
            }
        );
    }
}
