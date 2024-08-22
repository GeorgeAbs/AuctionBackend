using Domain.BackendResponses;
using Domain.Constants;
using Domain.CoreEnums;
using Domain.Entities.UserEntity;
using Domain.Interfaces;
using Domain.Interfaces.DbContexts;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly IUserDbContext _identityContext;
        private readonly UserManager<User> _userManager;
        public AuthService(ITokenService tokenService, IUserDbContext identityContext, UserManager<User> userManager) 
        {
            _tokenService = tokenService;
            _identityContext = identityContext;
            _userManager = userManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>access token, refresh token</returns>
        public async Task<MethodResult<string, string>> LoginAsync(string email, string password)
        {
            var dbUser = await _identityContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (dbUser is not null)
            {
                var res = _userManager.PasswordHasher.VerifyHashedPassword(dbUser, dbUser.PasswordHash, password);
                if (res != PasswordVerificationResult.Failed)
                {
                    var result = await _tokenService.GenerateTokenAsync(email);
                    var refreshToken = _tokenService.GenerateRefreshToken();

                    dbUser.SetRefreshToken(refreshToken, DateTime.UtcNow.AddDays(Settings.REFRESH_TOKEN_TIME_EXPIRE_DAYS));

                    await _identityContext.SaveChangesAsync();

                    return new MethodResult<string, string>(result.ResultEntity, refreshToken, result.Messages, result.Result);
                }

                else
                {
                    return new MethodResult<string, string>(null, null, [ResponsesTextConstants.LOGIN_OR_EMAIL_ARE_WRONG], Enums.MethodResults.Conflict);
                }
            }
            else
            {
                return new MethodResult<string, string>(null, null, [ResponsesTextConstants.LOGIN_OR_EMAIL_ARE_WRONG], Enums.MethodResults.Conflict);
            }
        }

        public async Task LogoutAsync(Guid userId)
        {
            var dbUser = await _identityContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (dbUser is null) return;

            dbUser.EraseRefreshToken();

            await _identityContext.SaveChangesAsync();
        }

        public async Task<MethodResult<string, string>> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            var result = _tokenService.GetEmailFromExpiredToken(accessToken);

            if (result.Result != Enums.MethodResults.Ok)
            {
                return new MethodResult<string, string>(null, null, result.Messages, Enums.MethodResults.Conflict);
            }

            var email = result.ResultEntity;

            var dbUser = await _identityContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if ( dbUser is null || dbUser.RefreshToken != refreshToken || dbUser.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                return new MethodResult<string, string>(null, null, [], Enums.MethodResults.Conflict);
            }

            var resultToken = await _tokenService.GenerateTokenAsync(email!);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            dbUser.SetRefreshToken(refreshToken, DateTime.UtcNow.AddDays(Settings.REFRESH_TOKEN_TIME_EXPIRE_DAYS));

            await _identityContext.SaveChangesAsync();

            return new MethodResult<string, string>(resultToken.ResultEntity, newRefreshToken, result.Messages, result.Result);

        }
    }
}
