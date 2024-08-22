namespace Domain.Interfaces.Services
{
    public interface IIdentifiersService
    {
        public Task<string> GetOrderNewIdentifierAsync();
    }
}
