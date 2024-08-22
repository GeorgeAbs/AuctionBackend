using Domain.BackendResponses;

namespace Domain.Interfaces.Services
{
    public interface IUserService<TDto, RUser> where TDto : class where RUser : class
    {
        Task<MethodResult> CreateAsync(TDto userDto);

        Task<MethodResult> ActivateAsync(string email, string activationCode);

        Task<MethodResult<RUser>> GetUserAsync(Guid userId);

        Task<MethodResult> ResendActivationCodeAsync(string email);

        Task<MethodResult> SendCodeForResetPasswordAsync(string email);

        Task<MethodResult> ResetPasswordAsync(string email, string resetPwdConfirmationCode, string newPwd1, string newPwd2);
    }

}
