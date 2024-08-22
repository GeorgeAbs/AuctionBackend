namespace Domain.Interfaces.Services
{
    public interface IAuctionEndingService
    {
        public Task ProceedEndedAuctionAsync();
    }
}
