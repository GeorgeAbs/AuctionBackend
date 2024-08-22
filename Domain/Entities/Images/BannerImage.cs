using Domain.CoreEnums;

namespace Domain.Entities.Images
{
    public class BannerImage : ImageBase
    {
        public Enums.BannerType BannerType { get; private set; }

        private BannerImage() { }

        public BannerImage(string bigImagePath, string smallImagePath, Enums.BannerType bannerType) : base(bigImagePath, smallImagePath)
        {
            BigImagePath = bigImagePath;
            SmallImagePath = smallImagePath;
            BannerType = bannerType;
        }
    }
}
