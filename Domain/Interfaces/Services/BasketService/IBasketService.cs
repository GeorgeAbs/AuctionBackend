using Domain.BackendResponses;
using Domain.Interfaces.Services.BasketService.Dto;

namespace Domain.Interfaces.Services.BasketService
{
    public interface IBasketService
    {
        public Task<BasketResponse> GetBasketAsync(Guid userId);

        public Task<MethodResult<BasketResponse>> AddItemAsync(Guid userId, Guid itemId, int quantity);

        public Task<BasketResponse> RemoveItemAsync(Guid userId, Guid itemId, int quantity);
    }
}
