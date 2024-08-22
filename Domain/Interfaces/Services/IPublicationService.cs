using Domain.BackendResponses;
using Domain.Entities.Items.ItemTrading;

namespace Domain.Interfaces.Services
{
    public interface IPublicationService
    {
        public Task<MethodResult> PublicAsync(Guid itemId);
    }
}
