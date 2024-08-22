using Domain.Common.Dto;
using Domain.Common.Filters;
using Domain.Constants;
using Domain.CoreEnums;
using Domain.Entities.UserEntity;
using Domain.Entities.UserEntity.DTO;
using Domain.Interfaces.Services;
using Domain.Interfaces.Services.MyProfileService;
using Domain.Interfaces.Services.MyProfileService.Dto;
using Domain.Interfaces.Services.MyProfileService.Dto.Address;
using Domain.Interfaces.Services.MyProfileService.Dto.Comment;
using Domain.Interfaces.Services.MyProfileService.Dto.History;
using Domain.Interfaces.Services.MyProfileService.Dto.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using WebApi.Dto.UserDto.Requests.UserInfoForMyProfile;
using WebApi.DTO.CommentsReviewFilter.Request;
using WebApi.DTO.CommentsReviewFilter.Response;
using WebApi.DTO.History.Requests;
using WebApi.DTO.HistoryFilter.Response;
using WebApi.DTO.UserDTO.Requests;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
    [Route("[controller]/[action]")]
    public class MyProfileController : ApiController
    {
        private readonly IUserService<UserDto, User> _userService;
        private readonly IMyProfileService _myProfileService;
        private readonly ILocalizer _localizer;

        public MyProfileController(IUserService<UserDto, User> userService, IMyProfileService myProfileService, ILocalizer localizer)
        {
            _userService = userService;
            _myProfileService = myProfileService;
            _localizer = localizer;
            _localizer.Initialize(Constants.Enums.Locale.Ru);
        }

        [Authorize(Roles =
        $"{RoleConstants.DEFAULT_USER}," +
        $"{RoleConstants.SUPER_USER}")]
        [HttpGet]
        [SwaggerOperation(null, "Get user short info page header")]
        [SwaggerResponse(200,
            "Returns user short info page header. User id will be retrieved from jwt. Only for default user",
            typeof(MyProfileUserShortInfoResponse))]
        [SwaggerResponse(409,
            "Returns messages if some error accures (user is not existed and so on)",
            typeof(List<string>))]
        public async Task<IActionResult> GetUserShortInfoAsync()
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Conflict();
            }

            if (HttpContext.Request.Headers.TryGetValue("доп заголовок с айди юзера", out var userIdStringValues))
            {
                var userIdFirstString = userIdStringValues.FirstOrDefault();
                if (!String.IsNullOrEmpty(userIdFirstString))
                {
                    Guid.TryParse(userIdFirstString, out userId);
                }
            }

            var res = await _myProfileService.GetUserShortInfoAsync(userId);

            if (res.Result == Enums.MethodResults.Conflict) return Conflict(res.Messages);

            return Ok(res.ResultEntity);
        }

        [Authorize(Roles =
        $"{RoleConstants.DEFAULT_USER}," +
        $"{RoleConstants.SUPER_USER}")]
        [HttpGet]
        [SwaggerOperation(null, "Get user info for My profile page")]
        [SwaggerResponse(200,
            "Returns full user info for My profile page. User id will be retrieved from jwt. Only for default user",
            typeof(MyProfileUserInfoResponse))]
        [SwaggerResponse(409,
            "Returns messages if some error accures (user is not existed and so on)",
            typeof(List<string>))]
        public async Task<IActionResult> GetUserInfoForMyProfileAsync()
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Conflict();
            }

            var res = await _myProfileService.GetUserInfoAsync(userId);

            if (res.Result == Enums.MethodResults.Conflict) return Conflict(res.Messages);



            return Ok(res.ResultEntity);
        }

        [Authorize(Roles =
        $"{RoleConstants.DEFAULT_USER}," +
        $"{RoleConstants.SUPER_USER}")]
        [HttpPut]
        [SwaggerOperation(null, "Change User logo")]
        [SwaggerResponse(200,
            "Returns new logo urls. User id will be retrieved from jwt. Require auth",
            typeof(ImageDto))]
        [SwaggerResponse(409,
            "Returns messages if some error accures (user is not existed and so on)",
            typeof(List<string>))]
        public async Task<IActionResult> ChangeUserLogoAsync([FromForm]IFormFile newLogo)
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Conflict(new List<string> {"Ошибка" });
            }

            var formCollection = await HttpContext.Request.ReadFormAsync();
            var files = formCollection.Files;
            //var imageStream = files.First().OpenReadStream();
            await Console.Out.WriteLineAsync("!!!!!!!!!!!" + files.Count);

            var imageStream = newLogo.OpenReadStream();
            //var imageStream = new MemoryStream();
            var result = await _myProfileService.ChangeUserLogoAsync(userId, imageStream);

            imageStream.Close();

            if (result.Result == Enums.MethodResults.Conflict) return Conflict(result.Messages);

            return Ok(result.ResultEntity);
        }

        [Authorize(Roles =
        $"{RoleConstants.DEFAULT_USER}," +
        $"{RoleConstants.SUPER_USER}")]
        [HttpPut]
        [SwaggerOperation(null, "Change Shop logo")]
        [SwaggerResponse(200,
            "Returns new logo urls. User id will be retrieved from jwt. Only for default user",
            typeof(ImageDto))]
        [SwaggerResponse(409,
            "Returns messages if some error accures (user is not existed and so on)",
            typeof(List<string>))]
        public async Task<IActionResult> ChangeShopLogoAsync(IFormFile newLogo)
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Conflict(new List<string> { "Ошибка" });
            }

            var imageStream = newLogo.OpenReadStream();

            var result = await _myProfileService.ChangeShopLogoAsync(userId, imageStream);

            imageStream.Close();

            if (result.Result == Enums.MethodResults.Conflict) return Conflict(result.Messages);

            return Ok(result.ResultEntity);
        }

        [Authorize(Roles =
        $"{RoleConstants.DEFAULT_USER}," +
        $"{RoleConstants.SUPER_USER}")]
        [HttpPut]
        [SwaggerOperation(null, "Change user name")]
        [SwaggerResponse(200,
            "Returns messages of success. User id will be retrieved from jwt. Only for default user",
            typeof(List<string>))]
        [SwaggerResponse(409,
            "Returns messages if some error accures (user is not existed and so on)",
            typeof(List<string>))]
        public async Task<IActionResult> ChangeUserNameAsync([FromBody] StringBody stringBody)
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Conflict(new List<string> { "Ошибка"});
            }

            var result = await _myProfileService.ChangeUserNameAsync(userId, stringBody.StringContent);

            if (result.Result == Enums.MethodResults.Conflict) return Conflict(result.Messages);

            return Ok(result.Messages);
        }

        [Authorize(Roles =
        $"{RoleConstants.DEFAULT_USER}," +
        $"{RoleConstants.SUPER_USER}")]
        [HttpPut]
        [SwaggerOperation(null, "Change user name")]
        [SwaggerResponse(200,
            "Returns messages of success. User id will be retrieved from jwt. Only for default user",
            typeof(List<string>))]
        [SwaggerResponse(409,
            "Returns messages if some error accures (user is not existed and so on)",
            typeof(List<string>))]
        public async Task<IActionResult> ChangeUserInfoAsync([FromBody] RequestUserInfoForMyProfile userProfile)
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Conflict();
            }

            var result = await _myProfileService.ChangeUserInfoAsync(userId: userId,
                firstName: userProfile.FirstName,
                secondName: userProfile.SecondName,
                birthDate: userProfile.BirthDate,
                vkLink: userProfile.VkLink,
                facebookLink: userProfile.FacebookLink,
                telegramLink: userProfile.TelegramLink,
                whatsappLink: userProfile.WhatsappLink,
                isUserAsShopOption: userProfile.IsUserAsShopOption,
                shopTitle: userProfile.ShopTitle,
                shopDescription: userProfile.ShopDescription,
                daysForShipment: userProfile.DaysForShipment);

            if (result.Result == Enums.MethodResults.Conflict) return Conflict(result.Messages);

            return Ok(result.Messages);
        }

        [Authorize(Roles =
        $"{RoleConstants.DEFAULT_USER}," +
        $"{RoleConstants.SUPER_USER}")]
        [HttpPut]
        [SwaggerOperation(null, "Change pwd")]
        [SwaggerResponse(200,
            "Returns messages of success. User id will be retrieved from jwt. Only for default user",
            typeof(List<string>))]
        [SwaggerResponse(409,
            "Returns messages if some error accures (user is not existed and so on)",
            typeof(List<string>))]
        public async Task<IActionResult> ChangePwdAsync([FromBody] ChangePwdRequest request)
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Conflict();
            }

            var result = await _myProfileService.ChangePwdAsync(userId, request.CurrendPwd, request.NewPwd);

            if (result.Result == Enums.MethodResults.Conflict) return Conflict(result.Messages);

            return Ok(result.Messages);
        }

        //наверно не нужно
        /*[Authorize(Roles =
        $"{RoleConstants.DEFAULT_USER}," +
        $"{RoleConstants.SUPER_USER}")]
        [HttpPut]
        [SwaggerOperation(null, "Change email")]
        [SwaggerResponse(200,
            "Returns messages of success. User id will be retrieved from jwt. Only for default user",
            typeof(List<string>))]
        [SwaggerResponse(409,
            "Returns messages if some error accures (user is not existed and so on)",
            typeof(List<string>))]
        public async Task<IActionResult> ChangeEmailAsync([FromBody] string newEmail)
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Conflict();
            }

            var result = await _myProfileService.ChangeEmailAsync(userId, newEmail);

            if (result.Result == Enums.MethodResults.Conflict) return Conflict(result.Messages);

            return Ok(result.Messages);


        }*/

        /*[Authorize(Roles =
        $"{RoleConstants.DEFAULT_USER}," +
        $"{RoleConstants.SUPER_USER}")]
        [HttpPut]
        [SwaggerOperation(null, "Confirm changing email")]
        [SwaggerResponse(200,
            "Returns messages of success. User id will be retrieved from jwt. Only for default user",
            typeof(List<string>))]
        [SwaggerResponse(409,
            "Returns messages if some error accures (user is not existed and so on)",
            typeof(List<string>))]
        public async Task<IActionResult> ConfirmChangingEmailAsync([FromBody] string confirmationCode)
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Conflict();
            }

            var result = await _myProfileService.ConfirmChangingEmailAsync(userId, confirmationCode);

            if (result.Result == Enums.MethodResults.Conflict) return Conflict(result.Messages);

            return Ok(result.Messages);
        }*/

        /*[Authorize(Roles =
        $"{RoleConstants.DEFAULT_USER}," +
        $"{RoleConstants.SUPER_USER}")]
        [HttpPut]
        [SwaggerOperation(null, "Change email")]
        [SwaggerResponse(200,
            "Returns messages of success. User id will be retrieved from jwt. Only for default user",
            typeof(List<string>))]
        [SwaggerResponse(409,
            "Returns messages if some error accures (user is not existed and so on)",
            typeof(List<string>))]
        public async Task<IActionResult> ChangePhoneAsync([FromBody] string newPhone)
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Conflict();
            }

            var result = await _myProfileService.ChangePhoneAsync(userId, newPhone);

            if (result.Result == Enums.MethodResults.Conflict) return Conflict(result.Messages);

            return Ok(result.Messages);


        }*/

        /*[Authorize(Roles =
        $"{RoleConstants.DEFAULT_USER}," +
        $"{RoleConstants.SUPER_USER}")]
        [HttpPut]
        [SwaggerOperation(null, "Confirm changing phone")]
        [SwaggerResponse(200,
            "Returns messages of success. User id will be retrieved from jwt. Only for default user",
            typeof(List<string>))]
        [SwaggerResponse(409,
            "Returns messages if some error accures (user is not existed and so on)",
            typeof(List<string>))]
        public async Task<IActionResult> ConfirmChangingPhoneAsync([FromBody] string confirmationCode)
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Conflict();
            }

            var result = await _myProfileService.ConfirmChangingEmailAsync(userId, confirmationCode);

            if (result.Result == Enums.MethodResults.Conflict) return Conflict(result.Messages);

            return Ok(result.Messages);
        }*/

        /*[Authorize(Roles =
        $"{RoleConstants.DEFAULT_USER}," +
        $"{RoleConstants.SUPER_USER}")]
        [HttpGet]
        [SwaggerOperation(null, "Get my orders history. User id will be retrieved from jwt. Only for default user")]
        [SwaggerResponse(200,
            "Returns history info",
            typeof(PaginationViewModel<MyProfileItemTradingHistoryEntity>))]
        [SwaggerResponse(409,
            "Returns messages if some error accures (user is not existed and so on)",
            typeof(List<string>))]
        public async Task<IActionResult> GetMyOrdersHistoryAsync([FromQuery] HistoryFilterRequest filter)
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Conflict();
            }

            var result = await _myProfileService.GetPurchaseHistoryAsync(userId,
                filter.PageNumber,
                filter.PageSize,
                filter.ItemStatus,
                filter.Sorting,
                filter.DateFrom,
                filter.DateTo);

            if (result.Result == Enums.MethodResults.Conflict) return Conflict(result.Messages);

            return Ok(result.ResultEntity);
        }*/

        [Authorize(Roles =
        $"{RoleConstants.DEFAULT_USER}," +
        $"{RoleConstants.SUPER_USER}")]
        [HttpGet]
        [SwaggerOperation(null, "Get my items history (items which user created, nor orders). User id will be retrieved from jwt. Only for default user")]
        [SwaggerResponse(200,
            "Returns history info",
            typeof(PaginationViewModel<MyProfileItemTradingHistoryEntity>))]
        [SwaggerResponse(409,
            "Returns messages if some error accures (user is not existed and so on)",
            typeof(List<string>))]
        public async Task<IActionResult> GetMyItemsHistoryAsync([FromQuery] HistoryFilterRequest filter)
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Conflict();
            }

            using var sw = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), "log.txt"), true);
            sw.Write("from GetMyItemsHistoryAsync request" + "\n");
            sw.Write(userId + "\n");
            sw.Write(filter.ItemStatus + "\n");

            var result = await _myProfileService.GetMyItemsHistoryAsync(userId,
                filter.PageNumber,
                filter.PageSize,
                filter.ItemStatus,
                filter.Sorting,
                filter.DateFrom,
                filter.DateTo);

            if (result.Result == Enums.MethodResults.Conflict) return Conflict(result.Messages);

            return Ok(result.ResultEntity);
        }

        /*[Authorize(Roles =
        $"{RoleConstants.DEFAULT_USER}," +
        $"{RoleConstants.SUPER_USER}")]
        [HttpGet]
        [SwaggerOperation(null, "Get auction history. User id will be retrieved from jwt. Only for default user")]
        [SwaggerResponse(200,
            "Returns history info",
            typeof(PaginationViewModel<MyProfileItemTradingHistoryEntity>))]
        [SwaggerResponse(409,
            "Returns messages if some error accures (user is not existed and so on)",
            typeof(List<string>))]
        public async Task<IActionResult> GetAuctionHistoryAsync([FromQuery] HistoryFilterRequest filter)
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Conflict();
            }

            var result = await _myProfileService.GetAuctionHistoryAsync(userId,
                filter.PageNumber,
                filter.PageSize,
                filter.ItemStatus,
                filter.Sorting,
                filter.DateFrom,
                filter.DateTo);

            if (result.Result == Enums.MethodResults.Conflict) return Conflict(result.Messages);

            return Ok(result.ResultEntity);
        }*/

        [AllowAnonymous]
        [HttpGet]
        [SwaggerOperation(null, "Get my orders filter. No auth required")]
        [SwaggerResponse(200,
            "Returns history filter",
            typeof(HistoryFilterResponse))]
        public IActionResult GetMyOrdersHistoryFilterAsync()
        {

            HistoryFilterResponse filter = new(_localizer, Filters.ItemTradingStatusesMyOrdersFilterForUI);

            return Ok(filter);
        }

        [AllowAnonymous]
        [HttpGet]
        [SwaggerOperation(null, "Get my items history filter. No auth required")]
        [SwaggerResponse(200,
            "Returns history filter",
            typeof(HistoryFilterResponse))]
        public IActionResult GetMyItemsHistoryFilterAsync()
        {
            HistoryFilterResponse filter = new(_localizer, Filters.ItemTradingStatusesMyItemsFilterForUI);

            return Ok(filter);
        }

        [AllowAnonymous]
        [HttpGet]
        [SwaggerOperation(null, "Get auction filter. No auth required")]
        [SwaggerResponse(200,
            "Returns history filter",
            typeof(HistoryFilterResponse))]
        public IActionResult GetAuctionHistoryFilterAsync()
        {
            HistoryFilterResponse filter = new(_localizer,  Filters.ItemTradingStatusesAuctionFilterForUI);

            return Ok(filter);
        }

        [Authorize(Roles =
        $"{RoleConstants.DEFAULT_USER}," +
        $"{RoleConstants.SUPER_USER}")]
        [HttpGet]
        [SwaggerOperation(null, "Get auction history. User id will be retrieved from jwt. Only for default user")]
        [SwaggerResponse(200,
            "Returns history info",
            typeof(PaginationViewModel<MyProfileReviewResponseEntity>))]
        [SwaggerResponse(409,
            "Returns messages if some error accures (user is not existed and so on)",
            typeof(List<string>))]
        public async Task<IActionResult> GetProductsReviewsAsync([FromQuery] CommentsReviewsFilterRequest filter)
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Conflict();
            }

            var result = await _myProfileService.GetProductsReviewsAsync(userId,
                filter.PageNumber,
                filter.PageSize,
                filter.ReviewCommentsType,
                filter.Sorting,
                filter.DateFrom,
                filter.DateTo);

            if (result.Result == Enums.MethodResults.Conflict) return Conflict(result.Messages);

            return Ok(result.ResultEntity);
        }

        [Authorize(Roles =
        $"{RoleConstants.DEFAULT_USER}," +
        $"{RoleConstants.SUPER_USER}")]
        [HttpGet]
        [SwaggerOperation(null, "Get auction history. User id will be retrieved from jwt. Only for default user")]
        [SwaggerResponse(200,
            "Returns history info",
            typeof(PaginationViewModel<MyProfileCommentResponseEntity>))]
        [SwaggerResponse(409,
            "Returns messages if some error accures (user is not existed and so on)",
            typeof(List<string>))]
        public async Task<IActionResult> GetProductsCommentsAsync([FromQuery] CommentsReviewsFilterRequest filter)
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Conflict();
            }

            var result = await _myProfileService.GetProductsCommentsAsync(userId,
                filter.PageNumber,
                filter.PageSize,
                filter.ReviewCommentsType,
                filter.Sorting,
                filter.DateFrom,
                filter.DateTo);

            if (result.Result == Enums.MethodResults.Conflict) return Conflict(result.Messages);

            return Ok(result.ResultEntity);
        }

        [Authorize(Roles =
        $"{RoleConstants.DEFAULT_USER}," +
        $"{RoleConstants.SUPER_USER}")]
        [HttpGet]
        [SwaggerOperation(null, "Get filter for comments or reviews. Only for default user")]
        [SwaggerResponse(200,
            "Returns filter for comments or reviews",
            typeof(CommentsReviewFilterResponse))]
        public IActionResult GetProductsReviewCommentsFilterAsync()
        {
            CommentsReviewFilterResponse filter = new(_localizer, Filters.ReviewCommentsFilter);

            return Ok(filter);
        }

        [Authorize(Roles =
        $"{RoleConstants.DEFAULT_USER}," +
        $"{RoleConstants.SUPER_USER}")]
        [HttpGet]
        [SwaggerOperation(null, "Returns all user adresses. Only for default user")]
        [SwaggerResponse(200,
            "Returns adresses",
            typeof(IEnumerable<MyProfileAddressDto>))]
        [SwaggerResponse(409,
            "Returns messages if some error accures (user is not existed and so on)",
            typeof(List<string>))]
        public async Task<IActionResult> GetAddressesAsync()
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Conflict();
            }

            var result = await _myProfileService.GetUserAddressesAsync(userId);

            if (result.Result == Enums.MethodResults.Conflict) return Conflict(result.Messages);

            return Ok(result.ResultEntity);
        }

        [Authorize(Roles =
        $"{RoleConstants.DEFAULT_USER}," +
        $"{RoleConstants.SUPER_USER}")]
        [HttpPost]
        [SwaggerOperation(null, "Creates new address or updates. Only for default user")]
        [SwaggerResponse(200,
            "Returns success messages",
            typeof(List<string>))]
        [SwaggerResponse(409,
            "Returns messages if some error accures (user is not existed and so on)",
            typeof(List<string>))]
        public async Task<IActionResult> SaveAddressAsync([FromBody]MyProfileAddressDto addressDto)
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Conflict();
            }

            var result = await _myProfileService.CreateChangeAddressAsync(userId, addressDto);

            if (result.Result == Enums.MethodResults.Conflict) return Conflict(result.Messages);

            return Ok(result.Messages);
        }

        [Authorize(Roles =
        $"{RoleConstants.DEFAULT_USER}," +
        $"{RoleConstants.SUPER_USER}")]
        [HttpDelete]
        [SwaggerOperation(null, "Deletes  address. Only for default user")]
        [SwaggerResponse(200,
            "Returns success messages",
            typeof(List<string>))]
        [SwaggerResponse(409,
            "Returns messages if some error accures (user is not existed and so on)",
            typeof(List<string>))]
        public async Task<IActionResult> DeleteAddressAsync([FromBody] Guid addressId)
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Conflict();
            }

            var result = await _myProfileService.DeleteAddressAsync(userId, addressId);

            if (result.Result == Enums.MethodResults.Conflict) return Conflict(result.Messages);

            return Ok(result.Messages);
        }

        [Authorize(Roles =
        $"{RoleConstants.DEFAULT_USER}," +
        $"{RoleConstants.SUPER_USER}")]
        [HttpGet]
        [SwaggerOperation(null, "Gets filter and all info to create/change item. Only for default user")]
        [SwaggerResponse(200,
            "Filter",
            typeof(MyProfileItemTradingCreationChangingFilter))]
        [SwaggerResponse(409,
            "Returns messages if some error accures (user is not existed and so on)",
            typeof(List<string>))]
        public async Task<IActionResult> GetCreateChangeFilterAsync()
        {
            if (!Guid.TryParse(HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Conflict();
            }

            var result = await _myProfileService.GetFilterForItemCreationChangingAsync(userId);

            if (result.Result == Enums.MethodResults.Conflict) return Conflict(result.Messages);

            return Ok(result.ResultEntity);
        }

        public class StringBody
        {
            public string StringContent { get; set; }

            public StringBody() { }
        }
    }
}