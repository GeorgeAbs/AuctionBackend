using Domain.Constants;
using Domain.Entities.Basket.DTO;
using Domain.Entities.Items.ItemTrading;
using Domain.Interfaces.Services.BasketService;
using Domain.Interfaces.Services.BasketService.Dto;
using Domain.Interfaces.Services.ItemService.ItemTradingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using WebApi.ViewModels.Basket;

namespace WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize(Roles =
        $"{RoleConstants.DEFAULT_USER}," +
        $"{RoleConstants.SUPER_USER}")]
    public class BasketController : ApiController
    {
        private readonly IBasketService _basketService;
        private readonly IItemTradingService<ItemTrading> _itemTradingService;

        public BasketController(IBasketService basketService, IItemTradingService<ItemTrading> itemTradingService)
        { 
            _basketService = basketService;
            _itemTradingService = itemTradingService;
        }

        [HttpGet]
        [SwaggerOperation(null, "Get user basket. User id is obtained from jwt. Only for default user")]
        [SwaggerResponse(200, "Returns basket obj", typeof(BasketResponse))]
        public async Task<IResult> GetBasketAsync()
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Results.Unauthorized();
            }

            var basket = await _basketService.GetBasketAsync(userId);

            return Results.Ok(basket);
        }

        [HttpPut]
        [SwaggerOperation(null, "Add item to basket. Default amount is 1. Basket is found by user id that is obtained from jwt. Only for default user")]
        [SwaggerResponse(200, "Returns basket obj", typeof(BasketResponse))]
        [SwaggerResponse(409, "Returns nothing if provided item is not found")]
        public async Task<IResult> AddItemAsync([FromBody] AddRemoveBasketItemViewModel model)
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Results.Unauthorized();
            }

            var res = await _basketService.AddItemAsync(userId, model.ItemTradingId, model.Amount);

            if (res.Result == Domain.CoreEnums.Enums.MethodResults.Conflict) return Results.Conflict(res.Messages);

            return Results.Ok(res.ResultEntity);
        }

        [HttpPut]
        [SwaggerOperation(null, "Remove item to basket. Default amount is 1. Basket is found by user id that is obtained from jwt. Only for default user")]
        [SwaggerResponse(200, "Returns basket obj", typeof(BasketResponse))]
        [SwaggerResponse(401, "Returns nothing if user id is not found in jwt")]
        public async Task<IResult> RemoveItemAsync([FromBody] AddRemoveBasketItemViewModel model)
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Results.Unauthorized();
            }

            var basket = await _basketService.RemoveItemAsync(userId, model.ItemTradingId, model.Amount);

            return Results.Ok(basket);
        }
    }
}
