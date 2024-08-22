using Domain.CoreEnums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using WebApi.DTO.Auth.Requests;
using WebApi.DTO.Auth.Responses;

namespace WebApi.Controllers
{
    [Route("[controller]/[action]")]
    public class AuthenticationController :  ApiController
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService) 
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Produces("text/plain")]
        [SwaggerOperation(null, "Login by sending email and pwd. For everybody")]
        [SwaggerResponse(200,
            "Returns access and refresh tokens",
            typeof(AuthResponse))]
        [SwaggerResponse(409,
            "Returns message of error if some error accures (user is not existed,wrong pwd, etc.)",
            typeof(List<string>))]
        public async Task<IResult> LoginAsync([FromBody] AuthRequest dto)
        {
            if (dto is null)
            {
                return Results.Conflict("Invalid client request");
            }

            var result = await _authService.LoginAsync(dto.Email, dto.Password);

            if (result.Result == Enums.MethodResults.Conflict || result.ResultEntityFirst is null || result.ResultEntitySecond is null)
            {
                return Results.Conflict(result.Messages);
            }

            return Results.Ok(new AuthResponse(result.ResultEntityFirst, result.ResultEntitySecond));
        }

        [Authorize]
        [HttpPost]
        [Produces("text/plain")]
        [SwaggerOperation(null, "Logout")]
        [SwaggerResponse(200)]
        public async Task<IResult> LogoutAsync()
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Results.Conflict();
            }

            await _authService.LogoutAsync(userId);

            return Results.Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Produces("text/plain")]
        [SwaggerOperation(null, "Refresh token")]
        [SwaggerResponse(200,
            "Returns access and refresh tokens",
            typeof(AuthResponse))]
        [SwaggerResponse(409,
            "Returns message of error if some error accures",
            typeof(List<string>))]
        public async Task<IResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request)
        {
            var result = await _authService.RefreshTokenAsync(request.AccessToken, request.RefreshToken);

            if (result.Result == Enums.MethodResults.Conflict || result.ResultEntityFirst is null || result.ResultEntitySecond is null)
            {
                return Results.Conflict(result.Messages);
            }

            return Results.Ok(new AuthResponse(result.ResultEntityFirst, result.ResultEntitySecond));
        }
    }
}
