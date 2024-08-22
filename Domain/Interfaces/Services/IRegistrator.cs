namespace Domain.Interfaces.Services
{
    public interface IRegistrator<T>
    {
        Task SendActivationLinkAsync(T user, string activationCode);
        Task<bool> ActivateUserAsync(T user);
        Task<bool> RegistrateUserAsync(T user);
    }
}
