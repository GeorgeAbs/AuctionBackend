namespace Domain.Entities.Basket
{
    public class BasketItem : EntityBase
    {
        public Basket Basket { get; private set; }

        public Guid ItemId { get; private set; }

        public int Quantity { get; private set; } = 0;

        private BasketItem() { }

        public BasketItem(Basket basket, Guid itemId, int initialQuantity)
        {
            Basket = basket;
            ItemId = itemId;
            AddQuantity(initialQuantity);
        }

        /// <summary>
        /// Increase quantity
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns>New value if success, overwise before value</returns>
        public int AddQuantity(int quantity)
        {
            if (quantity < 0 || quantity > 1000000) return Quantity;
            if (Quantity + quantity > 1000000) return Quantity;

            Quantity += quantity;

            return Quantity;
        }

        /// <summary>
        /// Decrease quantity
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns>New value if seccess, overwise before value</returns>
        public int RemoveQuantity(int quantity)
        {
            if (quantity < 0) return Quantity;
            if (Quantity - quantity < 0) return Quantity = 0;

            Quantity -= quantity;

            return Quantity;
        }
    }
}
