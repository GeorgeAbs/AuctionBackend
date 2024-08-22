using Domain.Constants;
using Domain.CoreEnums;
using Domain.Entities.Items.ItemTrading;
using Domain.Entities.UserEntity;
using Domain.Interfaces.Services.ItemService.ItemTradingService;
using Domain.Interfaces.Services.ModerationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using WebApi.DTO.Moderation;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize(Roles = 
        $"{RoleConstants.MODERATOR}," +
        $"{RoleConstants.ADMIN}," +
        $"{RoleConstants.SUPER_USER}")]
    public class ModeratorController : ApiController
    {
        private readonly IModerationService<ItemTrading> _moderationService;
        private readonly UserManager<User> _userManager;
        private readonly IItemTradingService<ItemTrading> _itemTradingService;

        public ModeratorController(IModerationService<ItemTrading> moderationService, UserManager<User> userManager, IItemTradingService<ItemTrading> itemTradingService) 
        {
            _moderationService = moderationService;
            _userManager = userManager;
            _itemTradingService = itemTradingService;
        }

        [HttpGet]
        [SwaggerOperation(null, "Get next item for moderation. Only for moderator")]
        [SwaggerResponse(200,
            "Returns view model with item, comments from autocheck",
            typeof(ModerationViewModel<string>))] 
        public async Task<IResult> GetNextItem()
        {
            var res = await _itemTradingService.GetItemForModerationAsync();

            if (res.Result == Enums.MethodResults.Conflict) return Results.Conflict(res.Messages);

            return Results.Ok(res.ResultEntity);
        }

        [HttpPut]
        [SwaggerOperation(null,
            "Approve item. Only for moderator")]
        [SwaggerResponse(200,
            "Returns nothing")]
        [SwaggerResponse(409,
            "Returns fault messages",
            typeof(List<string>))]
        public async Task<IActionResult> ApproveAsync([FromBody] Guid itemId)
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Conflict();
            }

            var result = await _moderationService.ApproveAsync(itemId, userId);

            if (result.Result == Enums.MethodResults.Conflict) return Conflict(result.Messages);

            return Ok();
        }

        [HttpPut]
        [SwaggerOperation(null,
            "Disapprove item. Only for moderator")]
        [SwaggerResponse(200,
            "Returns nothing")]
        [SwaggerResponse(409,
            "Returns fault messages",
            typeof(List<string>))]
        public async Task<IActionResult> DisapproveAsync([FromBody] ModerationDisapproveRequest moderationDisapproveRequest)
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Conflict();
            }

            var result = await _moderationService.DisapproveAsync(moderationDisapproveRequest.ItemId, moderationDisapproveRequest.Message, userId);

            if (result.Result == Enums.MethodResults.Conflict) return Conflict(result.Messages);

            return Ok();
        }

        [HttpGet]
        [SwaggerOperation(null,
            "Gets total count of items which need to moderate. Only for moderator")]
        [SwaggerResponse(200,
            "Returns items count", typeof(int))]
        public async Task<IActionResult> GetModerationItemsCountAsync()
        {
            var result = await _moderationService.GetModerationItemsCountAsync();

            return Ok(result);
        }
    }
}
