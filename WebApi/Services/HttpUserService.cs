using Domain.Interfaces.Services;
using System.Security.Claims;

namespace WebApi.Services
{
    public class HttpUserService : IHttpUserService
    {
        public Guid UserId { get; }

        public bool IsAuthenticated { get; }

        public HttpUserService(IHttpContextAccessor httpContextAccessor)
        {
            if (Guid.TryParse(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid id))
            {
                UserId = id;
            }

            IsAuthenticated = id != Guid.Empty;
        }
    }
}
