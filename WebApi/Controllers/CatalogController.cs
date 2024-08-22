using Domain.Constants;
using Domain.Interfaces.Services.CatalogService;
using Domain.Interfaces.Services.CatalogService.Dto.Catalog;
using Domain.Interfaces.Services.CatalogService.Dto.PropsCreation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers
{
    [Route("[controller]/[action]")]
    public class CatalogController : ApiController
    {
        ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [Authorize(Roles =
        $"{RoleConstants.ADMIN}," +
        $"{RoleConstants.SUPER_USER}")]
        [AllowAnonymous]
        [HttpPost]
        [SwaggerOperation(null, "Create new catalog category if it doesnt exist by specified info. Only for admins")]
        [SwaggerResponse(200, "Returns messages", typeof(List<string>))]
        [SwaggerResponse(409, "Returns errors messages", typeof(List<string>))]
        public async Task<IResult> CreateCatalogCategoryAsync([FromBody] CatalogCategoryCreationRequest catalogCategoryDto)
        {

            var res = await _catalogService.CreateCatalogCategoryAsync(catalogCategoryDto);

            if (res.Result == Domain.CoreEnums.Enums.MethodResults.Conflict)
            {
                return Results.Conflict(res.Messages);
            }

            return Results.Ok(res.Messages);
        }

        [AllowAnonymous]
        [HttpPost]
        [SwaggerOperation(null, "Create new catalog category by specified info. For everybody")]
        [SwaggerResponse(200, "Returns catalog", typeof(CatalogResponse))]
        [SwaggerResponse(409, "Returns nothing if this is no catalog yet")]
        public async Task<IResult> GetCatalogAsync()
        {
            var catalogDto = await _catalogService.GetCatalogAsync();

            if (catalogDto == null) return Results.Conflict();

            return Results.Ok(catalogDto);
        }

        [Authorize(Roles =
        $"{RoleConstants.ADMIN}," +
        $"{RoleConstants.SUPER_USER}")]
        [AllowAnonymous]
        [HttpPost]
        [SwaggerOperation(null, "Create new catalog property name if it doesnt exist by specified info. Only for admins")]
        [SwaggerResponse(200, "Returns messages", typeof(List<string>))]
        [SwaggerResponse(409, "Returns errors messages", typeof(List<string>))]
        public async Task<IResult> CreateCatalogPropNameAsync([FromBody] CatalogPropertyNameCreationRequest request)
        {

            var res = await _catalogService.CreateCatalogPropertyNameAsync(request);

            if (res.Result == Domain.CoreEnums.Enums.MethodResults.Conflict)
            {
                return Results.Conflict(res.Messages);
            }
                
            return Results.Ok(res.Messages);
        }

        [Authorize(Roles =
        $"{RoleConstants.ADMIN}," +
        $"{RoleConstants.SUPER_USER}")]
        [AllowAnonymous]
        [HttpPost]
        [SwaggerOperation(null, "Create new catalog string property if it doesnt exist by specified info. Only for admins")]
        [SwaggerResponse(200, "Returns messages", typeof(List<string>))]
        [SwaggerResponse(409, "Returns errors messages", typeof(List<string>))]
        public async Task<IResult> CreateCatalogStringPropAsync([FromBody] CatalogStringPropertyCreationRequest request)
        {

            var res = await _catalogService.CreateCatalogPropertyAsync(request, null);

            if (res.Result == Domain.CoreEnums.Enums.MethodResults.Conflict)
            {
                return Results.Conflict(res.Messages);
            }

            return Results.Ok(res.Messages);
        }

        [Authorize(Roles =
        $"{RoleConstants.ADMIN}," +
        $"{RoleConstants.SUPER_USER}")]
        [AllowAnonymous]
        [HttpPost]
        [SwaggerOperation(null, "Create new catalog bool property if it doesnt exist by specified info. Only for admins")]
        [SwaggerResponse(200, "Returns messages", typeof(List<string>))]
        [SwaggerResponse(409, "Returns errors messages", typeof(List<string>))]
        public async Task<IResult> CreateCatalogBoolPropAsync([FromBody] CatalogBoolPropertyCreationRequest request)
        {

            var res = await _catalogService.CreateCatalogPropertyAsync(null, request);

            if (res.Result == Domain.CoreEnums.Enums.MethodResults.Conflict)
            {
                return Results.Conflict(res.Messages);
            }

            return Results.Ok(res.Messages);
        }

        //[Authorize(Roles =
        //$"{RoleConstants.ADMIN}," +
        //$"{RoleConstants.SUPER_USER}")]
        //[HttpPost]
        //[SwaggerOperation(null, "Create new catalog property if it doesnt exist by specified info. Only for admins")]
        //[SwaggerResponse(200, "Returns nothing")]
        //public async Task<IResult> CreateCatalogPropertyAsync([FromBody] CatalogPropertyNameDto catalogPropertyDto)
        //{
        //    await _catalogService.CreateCatalogPropertyNameAsync(catalogPropertyDto);

        //    return Results.Ok();
        //}
    }
}
