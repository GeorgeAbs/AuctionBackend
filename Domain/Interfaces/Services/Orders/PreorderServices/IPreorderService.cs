using Domain.BackendResponses;
using Domain.Interfaces.Services.Orders.PreorderServices.Dto;

namespace Domain.Interfaces.Services.Orders.PreorderServices
{
    public interface IPreorderService
    {
        public Task<MethodResult> CreatePreorderAsync(PreorderCreationRequest preorderCreationRequest);

        public Task<MethodResult> DeletePreorderAsync(Guid preorderId);
    }
}
