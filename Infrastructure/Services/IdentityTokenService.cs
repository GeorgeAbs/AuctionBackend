using Application;
using Domain.BackendResponses;
using Domain.Constants;
using Domain.CoreEnums;
using Domain.Entities.UserEntity;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services
{
    public class IdentityTokenService : ITokenService
    {
        UserManager<User> _userManager;
        public IdentityTokenService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        async public Task<MethodResult<string>> GenerateTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new MethodResult<string>(null, [ResponsesTextConstants.LOGIN_OR_EMAIL_ARE_WRONG], Enums.MethodResults.Conflict);
            }

            if (user.IsEmailActivated == false)
            {
                return new MethodResult<string>(null, [ResponsesTextConstants.EMAIL_IS_NOT_ACTIVATED], Enums.MethodResults.Conflict);
            }
            //and mobile phone in future

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim> { new Claim(ClaimTypes.Email, email) };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            var jwt = new JwtSecurityToken(
            issuer: "3d",
            audience: "3d",
            claims: claims,
            expires: DateTime.Now.Add(TimeSpan.FromMinutes(Settings.PWD_RESET_CODE_LIFETIME_MINUTES)),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("990d1b7f-434a-426a-bd1a-ad4877c18fc4")), SecurityAlgorithms.HmacSha256));

            return new MethodResult<string>(new JwtSecurityTokenHandler().WriteToken(jwt), [], Enums.MethodResults.Ok);

        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns>returns user email as MethodResult</returns>
        public MethodResult<string> GetEmailFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("990d1b7f-434a-426a-bd1a-ad4877c18fc4")),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            { 
                return new MethodResult<string>(null, ["Invalid token"], Enums.MethodResults.Conflict); 
            }

            return new MethodResult<string>(principal.Claims.First(x => x.Type == ClaimTypes.Email).Value,[], Enums.MethodResults.Ok);
        }

    }
}
