using Domain.Entities.Items.ItemTrading;

namespace Domain.Entities.Images
{
    public class ItemTradingImage : EntityImageBase<ItemTrading>
    {
        private ItemTradingImage() { }

        public ItemTradingImage(ItemTrading ownerEntity, string bigImagePath, string smallImagePath)
            : base(ownerEntity, bigImagePath, smallImagePath) 
        {
            ownerEntity.Images.Add(this);
        }
    }
}
