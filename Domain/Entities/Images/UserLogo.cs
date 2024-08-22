namespace Domain.Entities.Images
{
    public class UserLogo : EntityImageBase<Guid>
    {
        private UserLogo() { }
        public UserLogo(Guid userId, string bigImagePath, string smallImagePath)
            : base(userId, bigImagePath, smallImagePath) { }
    }
}
