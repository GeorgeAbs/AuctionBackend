using Domain.BackendResponses;
using Domain.Interfaces.DbContexts;
using Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class PublicationService : IPublicationService
    {
        private readonly ICatalogDbContext _context;
        public PublicationService(ICatalogDbContext context)
        {
            _context = context;
        }

        public async Task<MethodResult> PublicAsync(Guid itemId)
        {
            var item = await _context.ItemsTrading.FirstOrDefaultAsync(x => x.Id == itemId);

            if (item is null) return new MethodResult(["Ошибка публикации"], Domain.CoreEnums.Enums.MethodResults.Conflict);

            ///доделать!!!!
            return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Ok);
        }
    }
}
