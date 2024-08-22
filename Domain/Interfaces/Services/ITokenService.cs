using Domain.BackendResponses;

namespace Domain.Interfaces.Services
{
    public interface ITokenService
    {
        public Task<MethodResult<string>> GenerateTokenAsync(string email);

        public string GenerateRefreshToken();

        public MethodResult<string> GetEmailFromExpiredToken(string token);

    }
}
