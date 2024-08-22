namespace Domain.Entities.Images
{
    public abstract class ImageBase : EntityBase
    {
        public string BigImagePath { get; set; }

        public string SmallImagePath { get; set; }

        public ImageBase() { }

        public ImageBase(string bigImagePath, string smallImagePath)
        {
            BigImagePath = bigImagePath;
            SmallImagePath = smallImagePath;
        }
    }
}
