using Domain.CoreEnums;
using Domain.Entities.UserEntity;
using Domain.Entities.UserEntity.DTO;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Dto.UserDto.Responses.UserInfoForItem;

namespace WebApi.Controllers
{
    [Route("[controller]/[action]")]
    public class UserController : ApiController
    {
        readonly IUserService<UserDto, User> _userService;

        public UserController(IUserService<UserDto, User> userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [SwaggerOperation(null, "Create user by sending initial info")]
        [SwaggerResponse(200,
            "Returns message of result",
            typeof(List<string>))]
        [SwaggerResponse(409,
            "Returns message of error if some error accures (user is existed, wrong pwd formatm, etc.)",
            typeof(List<string>))]
        public async Task<IActionResult> Create([FromBody] UserRegistrationDto user)
        {
            UserDto userDto = new UserDto
            {
                Email = user.Email,
                Password = user.Password,
                UserName = user.UserName
            };

            var result = await _userService.CreateAsync(userDto);
            if (result.Result == Enums.MethodResults.Ok)
                return Ok(result.Messages);
            else
                return Conflict(result.Messages);
        }

        [AllowAnonymous]
        [HttpPost]
        [SwaggerOperation(null, "Activate user email by sending initial info")]
        [SwaggerResponse(200, 
            "Returns message of result",
            typeof(List<string>))]
        [SwaggerResponse(409, 
            "Returns message of error if some error accures (user is not existed, wrong activation code, etc.)",
            typeof(List<string>))]
        public async Task<IActionResult> ActivateAsync([FromBody] UserActivationDto user)
        {

            var result = await _userService.ActivateAsync(user.Email, user.ActivationCode);
            if (result.Result == Enums.MethodResults.Ok)
                return Ok(result.Messages);
            else
                return Conflict(result.Messages);
        }

        [AllowAnonymous]
        [HttpPost]
        [SwaggerOperation(null, "Resend email activation code")]
        [SwaggerResponse(200,
            "Returns message of result", 
            typeof(List<string>))]
        [SwaggerResponse(409,
            "Returns message of error if some error accures (user is not existed, etc.)",
            typeof(List<string>))]
        public async Task<IActionResult> ResendEmailActivationCodeAsync([FromBody] EmailDto dto)
        {

            var result = await _userService.ResendActivationCodeAsync(dto.Email);
            if (result.Result == Enums.MethodResults.Ok)
                return Ok(result.Messages);
            else
                return Conflict(result.Messages);
        }

        [AllowAnonymous]
        [HttpPost]
        [SwaggerOperation(null, "Send password reset code to specified email")]
        [SwaggerResponse(200,
            "Returns message of result",
            typeof(List<string>))]
        [SwaggerResponse(409,
            "Returns message of error if some error accures (user is not existed, etc.)",
            typeof(List<string>)    )]
        public async Task<IActionResult> SendCodeForResetPasswordAsync([FromBody] EmailDto dto)
        {
            var result = await _userService.SendCodeForResetPasswordAsync(dto.Email);
            if (result.Result == Enums.MethodResults.Ok)
                return Ok(result.Messages);
            else
                return Conflict(result.Messages);
        }

        [AllowAnonymous]
        [HttpPost]
        [SwaggerOperation(null, "Reset account password")]
        [SwaggerResponse(200,
            "Returns message of result",
            typeof(List<string>))]
        [SwaggerResponse(409,
            "Returns message of error if some error accures (user is not existed, " +
            "wrong pwd format, pwd 1 and pwd 2 are not equal, etc.)",
            typeof(List<string>))]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] UserPwdResetDto user)
        {
            await Console.Out.WriteLineAsync(user.ValidationResetPwdCode);
            var result = await _userService.ResetPasswordAsync(user.Email,user.ValidationResetPwdCode, user.NewPwd1, user.NewPwd2);
            if (result.Result == Enums.MethodResults.Ok)
                return Ok(result.Messages);
            else
                return Conflict(result.Messages);
        }

        [AllowAnonymous]
        [HttpGet]
        [SwaggerOperation(null, "Get user info for general item page")]
        [SwaggerResponse(200,
            "Returns user info for general item page got from item with specified id. " +
            "Info depends on userAsShop option state. " +
            "AllowAnonymous ",
            typeof(List<string>))]
        [SwaggerResponse(409,
            "Returns nothing if some error accures (user is not existed and so on)",
            typeof(ResponseUserInfoForItem))]
        public async Task<IActionResult> GetUserInfoForItemsAsync(Guid userId)
        {
            var user = (await _userService.GetUserAsync(userId)).ResultEntity;

            if (user == null) return Conflict();

            return Ok(ResponseUserInfoForItemMapper.MapToDto(user));
        }
    }
}
