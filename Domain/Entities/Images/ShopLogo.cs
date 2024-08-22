namespace Domain.Entities.Images
{
    public class ShopLogo : EntityImageBase<Guid>
    {
        private ShopLogo() { }
        public ShopLogo(Guid userId, string bigImagePath, string smallImagePath)
            : base(userId, bigImagePath, smallImagePath) { }
    }
}
