namespace Domain.Entities.UserEntity.DTO
{
    public class UserRegistrationDto
    {
        public string Password { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;
    }
}
