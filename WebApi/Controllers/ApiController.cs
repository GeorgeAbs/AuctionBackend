using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [SwaggerResponse(401,
            "Returns nothing (unauthorised)")]
    public class ApiController : ControllerBase
    {
        
    }
}
