using Domain.BackendResponses;
using Domain.Constants;
using Domain.CoreEnums;
using Domain.Entities.UserEntity;
using Domain.Entities.UserEntity.DTO;
using Domain.Interfaces.Services;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;

namespace Application.Services
{
    public class UserService<T, R> : IUserService<T, R> where T : UserDto where R : User
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IEmailSender _emailSender;
        public UserService(
            UserManager<User> userManager,
            ITokenService tokenService,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _emailSender = emailSender;
        }

        public async Task<MethodResult> CreateAsync(T userDto)
        {
            try
            {
                MailAddress m = new MailAddress(userDto.Email);
            }
            catch { return new MethodResult(["Неверный формат почты"], Enums.MethodResults.Conflict); }

            var existedUserByEmail = await _userManager.FindByEmailAsync(userDto.Email);
            if (existedUserByEmail != null)
            {
                return new MethodResult([ResponsesTextConstants.USER_WITH_THIS_EMAIL_IS_ALREADY_EXISTED_ERROR], Enums.MethodResults.Conflict);
            }

            var existedUserByUserName = await _userManager.FindByNameAsync(userDto.UserName);
            if (existedUserByUserName != null)
            {
                return new MethodResult([ResponsesTextConstants.USER_WITH_THIS_USER_NAME_IS_ALREADY_EXISTED_ERROR], Enums.MethodResults.Conflict);
            }

            var result = await CreateAccountAsync(userDto);
            if (result.ResultEntity == null)
            {
                return new MethodResult(result.Messages, Enums.MethodResults.Conflict);
            }


            await SendActivationCodeToMailAsync(result.ResultEntity.Email, result.ResultEntity.EmailConfirmationCode);

            result.ResultEntity.EmailConfirmationCodeSendingDate = DateTime.UtcNow;
            await _userManager.UpdateAsync(result.ResultEntity);
            await _userManager.SetEmailAsync(result.ResultEntity, result.ResultEntity.Email);

            return new MethodResult([ResponsesTextConstants.ACTIVATION_CODE_IS_SENDED_TO_MAIL], Enums.MethodResults.Ok);
        }

        public async Task<MethodResult<R>> GetUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return new MethodResult<R>(null, [ResponsesTextConstants.USER_WITH_THIS_ID_IS_NOT_FOUND], Enums.MethodResults.Conflict);

            else
                return new MethodResult<R>((R)user, [], Enums.MethodResults.Ok);
        }

        public async Task<MethodResult> ActivateAsync(string email, string emailActivationCode)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new MethodResult([ResponsesTextConstants.EMAIL_OR_ACTIVATION_CODE_ERROR], Enums.MethodResults.Conflict);
            }

            if (user.IsEmailActivated)
            {
                return new MethodResult([ResponsesTextConstants.USER_IS_ALREADY_ACTIVATED], Enums.MethodResults.Conflict);
            }

            //mobile phone in future
            if (emailActivationCode != user.EmailConfirmationCode)
            {
                return new MethodResult([ResponsesTextConstants.EMAIL_OR_ACTIVATION_CODE_ERROR], Enums.MethodResults.Conflict);
            }

            user.IsEmailActivated = true;
            var res = await _userManager.UpdateAsync(user);
            var res2 = await _userManager.SetEmailAsync(user, user.Email);

            if (!res.Succeeded || !res2.Succeeded)
            {
                return new MethodResult([ResponsesTextConstants.USER_ACTIVATION_ERROR], Enums.MethodResults.Conflict);
            }

            return new MethodResult([ResponsesTextConstants.USER_IS_CONFIRMED], Enums.MethodResults.Ok);
        }

        public async Task<MethodResult> ResendActivationCodeAsync(string email)
        {
            var dbUser = await _userManager.FindByEmailAsync(email);
            if (dbUser != null)
            {
                var newCode = GetNewCode().ToString();

                dbUser.SetEmailConfirmationCode(newCode);
                dbUser.EmailConfirmationCodeSendingDate = DateTime.UtcNow;
                var res = await _userManager.UpdateAsync(dbUser);
                var res2 = await _userManager.SetEmailAsync(dbUser, dbUser.Email);

                if (!res.Succeeded || !res2.Succeeded)
                {
                    return new MethodResult([ResponsesTextConstants.GENERAL_ERROR_MESSAGE], Enums.MethodResults.Conflict);
                }
                await SendActivationCodeToMailAsync(dbUser.Email, newCode);

                return new MethodResult([ResponsesTextConstants.ACTIVATION_CODE_IS_SENDED_TO_MAIL], Enums.MethodResults.Ok);
            }
            else
            {
                return new MethodResult([ResponsesTextConstants.GENERAL_ERROR_MESSAGE], Enums.MethodResults.Conflict);
            }
        }

        public async Task<MethodResult> SendCodeForResetPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new MethodResult([ResponsesTextConstants.EMAIL_IS_NOT_FOUND], Enums.MethodResults.Conflict);
            }

            var validationResetPwdCode = await _userManager.GeneratePasswordResetTokenAsync(user);

            await _emailSender.SendEmailAsync(email, "Регистрация", "<p>код для сброса пароля: " + validationResetPwdCode + "<p/>");

            return new MethodResult([ResponsesTextConstants.PASSWORD_RESET_CONFIRMATION_CODE_IS_SENDED], Enums.MethodResults.Ok);
        }

        public async Task<MethodResult> ResetPasswordAsync(string email, string validationResetPwdCode, string newPwd1, string newPwd2)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new MethodResult([ResponsesTextConstants.USER_NOT_FOUND], Enums.MethodResults.Conflict);
            }

            if (newPwd1 != newPwd2)
            {
                return new MethodResult([ResponsesTextConstants.PASSWORD_VS_SECOND_PASSWORD_ARE_NOT_EQUAL], Enums.MethodResults.Conflict);
            }

            var res = await _userManager.ResetPasswordAsync(user, validationResetPwdCode, newPwd1);
            await _userManager.SetEmailAsync(user, user.Email);
            if (!res.Succeeded)
            {
                res.Errors.ForEach(x => Console.Out.WriteLine(x.Description));

                return new MethodResult([ResponsesTextConstants.PASSWORD_RESET_ERROR], Enums.MethodResults.Conflict);
            }

            return new MethodResult([ResponsesTextConstants.PASSWORD_IS_RESET], Enums.MethodResults.Ok);
        }

        /// <summary>
        /// Create user account with default role
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        private async Task<MethodResult<User>> CreateAccountAsync(T userDto)
        {
            User newUser = new(
                userDto.UserName,
                DateTime.UtcNow,
                userDto.Email,
                GetNewCode().ToString(),
                GetNewCode().ToString());

            var result = await _userManager.CreateAsync(newUser, userDto.Password);

            await _userManager.SetEmailAsync(newUser, newUser.Email);

            foreach (var error in result.Errors)
            {
                await Console.Out.WriteLineAsync(error.Code);
            }

            if (!result.Succeeded)
            {
                return new MethodResult<User>(null, [ResponsesTextConstants.USER_CREATION_GENERAL_ERROR], Enums.MethodResults.Conflict);
            }

            var identityResult = await _userManager.AddToRoleAsync(newUser, RoleConstants.DEFAULT_USER);

            if (!identityResult.Succeeded)
            {
                return new MethodResult<User>(null, [ResponsesTextConstants.USER_CREATION_GENERAL_ERROR], Enums.MethodResults.Conflict);
            }

            return new MethodResult<User>(newUser, [], Enums.MethodResults.Ok);
        }

        private string GetNewCode()
        {
            return Random.Shared.Next(100000, 999999).ToString();
        }

        private async Task SendActivationCodeToMailAsync(string mailTo, string activationCode)
        {
            await _emailSender.SendEmailAsync(mailTo, "Регистрация", "<p>код для активации: " + activationCode + "<p/>");
        }

    }
}
