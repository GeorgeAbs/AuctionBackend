namespace Domain.Entities.Images
{
    public class AttachmentImage<T> : EntityImageBase<T> where T : EntityBase
    {
        public AttachmentImage() { }

        public AttachmentImage(T ownerEntity, string bigImagePath, string smallImagePath)
            : base(ownerEntity, bigImagePath, smallImagePath) { }
    }
}
