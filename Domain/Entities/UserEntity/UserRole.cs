using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.UserEntity
{
    public class UserRole : IdentityRole<Guid>
    {
        public UserRole() { }

        public UserRole(string roleName) : base(roleName) { }
    }
}
