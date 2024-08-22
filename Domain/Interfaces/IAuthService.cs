using Domain.BackendResponses;

namespace Domain.Interfaces
{
    public interface IAuthService
    {
        public Task<MethodResult<string, string>> LoginAsync(string email, string password);

        public Task LogoutAsync(Guid userId);

        public Task<MethodResult<string, string>> RefreshTokenAsync(string accessToken, string refreshToken);
    }
}
