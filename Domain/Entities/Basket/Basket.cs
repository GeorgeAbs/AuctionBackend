using Domain.Entities.Items.ItemTrading;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.Basket
{
    public class Basket : EntityBase
    {
        public Guid UserId { get; private set; }

        private readonly List<BasketItem> _items = new();

        [BackingField(nameof(_items))]
        public IEnumerable<BasketItem> Items { get { return _items; } }

        private Basket() { }

        public Basket(Guid userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// Add item with specified quantity
        /// </summary>
        public void AddItem(Guid itemId, int quantity)
        {
            var existedItem = _items.FirstOrDefault(x => x.ItemId == itemId);

            if (existedItem is not null)
            {
                existedItem.AddQuantity(quantity);
            }

            else
            {
                _items.Add(new BasketItem(this, itemId, quantity));
            }
        }

        /// <summary>
        /// Remove item with specified quantity
        /// </summary>
        public void RemoveItem(Guid itemId, int quantity)
        {
            var existedItem = Items.FirstOrDefault(x => x.ItemId == itemId);

            if (existedItem is null) return;

            if (existedItem.Quantity <= quantity)
            {
                _items.Remove(existedItem);
                return;
            }

            existedItem.RemoveQuantity(quantity);
        }

        /// <summary>
        /// Delete item from basket
        /// </summary>
        public void DeleteItem(Guid itemId)
        {
            var existedItem = Items.FirstOrDefault(x => x.ItemId == itemId);

            if (existedItem == null) return;

            _items.Remove(existedItem);
        }
    }
}
