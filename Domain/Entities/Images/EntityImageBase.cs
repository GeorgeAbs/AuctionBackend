namespace Domain.Entities.Images
{
    public abstract class EntityImageBase<T> : ImageBase
    {
        /// <summary>
        /// might be itemTrading, user, char message, etc...
        /// </summary>
        public T OwnerEntity { get; set; }

        public EntityImageBase() { }

        public EntityImageBase(T ownerEntity, string bigImagePath, string smallImagePath) : base (bigImagePath, smallImagePath)
        {
            BigImagePath = bigImagePath;
            SmallImagePath = smallImagePath;
            OwnerEntity = ownerEntity;
        }
    }
}
