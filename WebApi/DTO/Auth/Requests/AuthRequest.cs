namespace WebApi.DTO.Auth.Requests
{
    public class AuthRequest
    {
        public string Email { get; }

        public string Password { get;}

        public AuthRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
