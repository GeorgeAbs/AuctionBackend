using Domain.BackendResponses;
using Domain.Common.Dto;
using Domain.Common.Filters;
using Domain.Common.Sorting;
using Domain.CoreEnums;
using Domain.Entities.Addresses;
using Domain.Entities.Catalog;
using Domain.Entities.Comments;
using Domain.Entities.Items.ItemTrading;
using Domain.Entities.Reviews;
using Domain.Entities.UserEntity;
using Domain.Interfaces.DbContexts;
using Domain.Interfaces.Services;
using Domain.Interfaces.Services.MyProfileService;
using Domain.Interfaces.Services.MyProfileService.Dto;
using Domain.Interfaces.Services.MyProfileService.Dto.Address;
using Domain.Interfaces.Services.MyProfileService.Dto.Catalog;
using Domain.Interfaces.Services.MyProfileService.Dto.Comment;
using Domain.Interfaces.Services.MyProfileService.Dto.History;
using Domain.Interfaces.Services.MyProfileService.Dto.Review;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Domain.CoreEnums.Enums;

namespace Application.Services.MyProfile
{
    public class MyProfileService : IMyProfileService
    {
        private readonly IUserDbContext _userDbContext;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ICatalogDbContext _catalogContext;
        private readonly IImageService _imageService;

        public MyProfileService(IUserDbContext applicationIdentityContext,
            UserManager<User> userManager,
            IEmailSender emailSender,
            ICatalogDbContext catalogContext,
            IImageService imageService)
        {
            _userDbContext = applicationIdentityContext;
            _userManager = userManager;
            _emailSender = emailSender;
            _catalogContext = catalogContext;
            _imageService = imageService;
        }

        public async Task<MethodResult<MyProfileUserInfoResponse>> GetUserInfoAsync(Guid userId)
        {
            var user = await _userDbContext.Users
                .Include(x => x.Addresses)
                .Include(x => x.PaymentMethods)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null) return new(null, ["Пользователь не найден"], MethodResults.Conflict);

            var roles = await _userManager.GetRolesAsync(user);

            var info = new MyProfileUserInfoResponse();
            info.SetInfo(userName: user.UserName,
                firstName: user.FirstName,
                secondName: user.SecondName,
                birthDate: user.BirthDate,
                email: user.Email,
                phoneNumber: user.PhoneNumber,
                vkLink: user.VkLink,
                facebookLink: user.FacebookLink,
                telegramLink: user.TelegramLink,
                whatsappLink: user.WhatsappLink,
                userLogoSmallImage: user.UserLogoSmallImage,
                userLogoBigImage: user.UserLogoBigImage,
                isUserAsShopOption: user.IsUserAsShopOption,
                shopTitle: user.ShopTitle,
                shopDescription: user.ShopDescription,
                shopLogoSmallImage: user.ShopLogoSmallImage,
                shopLogoBigImage: user.ShopLogoBigImage,
                defaultDaysForShipment: user.DefaultDaysForShipment,
                roles: roles.ToList());

            return new MethodResult<MyProfileUserInfoResponse>(info, [], MethodResults.Ok);
        }

        public async Task<MethodResult<MyProfileUserShortInfoResponse>> GetUserShortInfoAsync(Guid userId)
        {
            var user = await _userDbContext.Users
                .Include(x => x.Addresses)
                .Include(x => x.PaymentMethods)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null) return new(null, ["Пользователь не найден"], MethodResults.Conflict);

            var info = new MyProfileUserShortInfoResponse();
            info.SetInfo(userName: user.UserName,
                userLogoSmallImage: user.UserLogoSmallImage,
                userLogoBigImage: user.UserLogoBigImage,
                isUserAsShopOption: user.IsUserAsShopOption,
                shopTitle: user.ShopTitle,
                shopDescription: user.ShopDescription,
                shopLogoSmallImage: user.ShopLogoSmallImage,
                shopLogoBigImage: user.ShopLogoBigImage);

            return new MethodResult<MyProfileUserShortInfoResponse>(info, [], MethodResults.Ok);
        }

        /// <summary>
        /// returns result entity as new logo path
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public async Task<MethodResult<ImageDto>> ChangeUserLogoAsync(Guid userId, Stream fileStream)
        {
            var user = await _userDbContext.Users.FindAsync(userId);

            if (user == null) return new(null, [], MethodResults.Conflict);

            var oldBigPicture = new string(user.UserLogoBigImage);
            var oldSmallPicture = new string(user.UserLogoSmallImage);

            var bigImage = await _imageService.SaveImageAsync(ImagePurpose.BigImage, fileStream);
            var smallImage = await _imageService.SaveImageAsync(ImagePurpose.SmallImage, fileStream);

            user.ChangeUserLogo(bigImage.ResultEntity!, smallImage.ResultEntity!);

            if (bigImage.Result != MethodResults.Ok || smallImage.Result != MethodResults.Ok)
            {
                var messages = new List<string>();
                messages.AddRange(bigImage.Messages);
                messages.AddRange(smallImage.Messages);
                return new MethodResult<ImageDto>(null, messages, MethodResults.Conflict);
            }

            await _imageService.DeleteImageAsync(oldBigPicture);
            await _imageService.DeleteImageAsync(oldSmallPicture);

            await _userDbContext.SaveChangesAsync();

            return new(new ImageDto(user.UserLogoBigImage, user.UserLogoSmallImage),[], MethodResults.Ok);
        }

        /// <summary>
        /// returns result entity as new logo path
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public async Task<MethodResult<ImageDto>> ChangeShopLogoAsync(Guid userId, Stream fileStream)
        {
            var user = await _userDbContext.Users.FindAsync(userId);

            if (user == null) return new(null, [], MethodResults.Conflict);

            var oldBigPicture = new string(user.ShopLogoBigImage);
            var oldSmallPicture = new string(user.ShopLogoSmallImage);

            var bigImage = await _imageService.SaveImageAsync(ImagePurpose.BigImage, fileStream);
            var smallImage = await _imageService.SaveImageAsync(ImagePurpose.SmallImage, fileStream);

            if (bigImage.Result != MethodResults.Ok || smallImage.Result != MethodResults.Ok)
            {
                var messages = new List<string>();
                messages.AddRange(bigImage.Messages);
                messages.AddRange(smallImage.Messages);
                return new MethodResult<ImageDto>(null, messages, MethodResults.Conflict);
            }

            await _imageService.DeleteImageAsync(oldBigPicture);
            await _imageService.DeleteImageAsync(oldSmallPicture);

            user.ChangeShopLogo(bigImage.ResultEntity!, smallImage.ResultEntity!);

            await _userDbContext.SaveChangesAsync();

            return new(new ImageDto(user.ShopLogoBigImage, user.ShopLogoSmallImage), [], MethodResults.Ok);
        }

        public async Task<MethodResult> ChangeUserNameAsync(Guid userId, string newUserName)
        {
            if (newUserName == null || newUserName == "") return new(["User Name не может быть пустым"], MethodResults.Conflict);//name is in using

            var user = await _userDbContext.Users.FindAsync(userId);

            if (user == null) return new([], MethodResults.Conflict);

            if ((await _userManager.FindByNameAsync(newUserName)) != null) return new(["User Name уже используется"], MethodResults.Conflict);//name is in using

            var result = await _userManager.SetUserNameAsync(user, newUserName);

            if (!result.Succeeded) return new(["Ошибка смена User Name"], MethodResults.Conflict);

            await _userManager.UpdateNormalizedUserNameAsync(user);

            await _userManager.UpdateNormalizedEmailAsync(user);

            return new([], MethodResults.Ok);
        }

        public async Task<MethodResult> ChangeUserInfoAsync(Guid userId,
            string firstName,
            string secondName,
            DateTime birthDate,
            string vkLink,
            string facebookLink,
            string telegramLink,
            string whatsappLink,
            bool isUserAsShopOption,
            string shopTitle,
            string shopDescription,
            int daysForShipment)
        {
            var user = await _userDbContext.Users.FindAsync(userId);

            if (user == null) return new([], MethodResults.Conflict);

            user.ChangeUserInfo(firstName,
            secondName,
            birthDate.ToUniversalTime(),
            vkLink,
            facebookLink,
            telegramLink,
            whatsappLink,
            isUserAsShopOption,
            shopTitle,
            shopDescription,
            daysForShipment);

            //need validation of country, city by some lists
            await _userDbContext.SaveChangesAsync();
            return new([], MethodResults.Ok);
        }

        public async Task<MethodResult> ChangePwdAsync(Guid userId, string oldPwd, string newPwd)
        {
            var user = await _userDbContext.Users.FindAsync(userId);

            if (user == null) return new([], MethodResults.Conflict);

            var result = await _userManager.ChangePasswordAsync(user, oldPwd, newPwd);

            if (!result.Succeeded) return new([], MethodResults.Conflict);

            return new([], MethodResults.Ok);
        }

        public async Task<MethodResult> ChangeEmailAsync(Guid userId, string newEmail)
        {
            var user = await _userDbContext.Users.FindAsync(userId);

            if (user == null) return new([], MethodResults.Conflict);

            if (await _userManager.FindByEmailAsync(newEmail) != null) return new([], MethodResults.Conflict);//email is in using

            var code = Random.Shared.Next(100000, 999999).ToString();

            await _emailSender.SendEmailAsync(newEmail, "Подтверждение смены почты", "<p>код для активации: " + code + "<p/>");

            user.SetEmailConfirmationCode(code);

            user.SetTempEmail(newEmail);

            await _userDbContext.SaveChangesAsync();

            return new([], MethodResults.Ok);
        }

        public async Task<MethodResult> ConfirmChangingEmailAsync(Guid userId, string confirmationCode)
        {
            var user = await _userDbContext.Users.FindAsync(userId);

            if (user == null) return new([], MethodResults.Conflict);

            if (await _userManager.FindByEmailAsync(user.TemporaryEmail) != null) return new([], MethodResults.Conflict);//email is in using

            if (confirmationCode != user.EmailConfirmationCode) return new([], MethodResults.Conflict);//code is wrong

            await _userManager.SetEmailAsync(user, user.TemporaryEmail);

            await _userManager.UpdateNormalizedEmailAsync(user);

            return new([], MethodResults.Ok);
        }

        public async Task<MethodResult> ChangePhoneAsync(Guid userId, string phone)
        {
            var user = await _userDbContext.Users.FindAsync(userId);

            if (user == null) return new([], MethodResults.Conflict);



            return new([], MethodResults.Ok);
        }

        public async Task<MethodResult> ConfirmChangingPhoneAsync(Guid userId, string confirmationCode)
        {
            var user = await _userDbContext.Users.FindAsync(userId);

            if (user == null) return new([], MethodResults.Conflict);



            return new([], MethodResults.Ok);
        }

        public async Task<MethodResult<PaginationViewModel<MyProfileItemTradingHistoryEntity>>> GetPurchaseHistoryAsync(Guid userId,
            int pageNumber,
            int pageSize,
            ItemTradingStatus itemStatus,
            SortingMethods sorting,
            DateTime dateFrom,
            DateTime dateTo)
        {
            var items = _catalogContext.ItemsTrading
                .Include(x => x.Images)
                .Where(x =>
                    x.CustomerId == userId && //customer, not owner
                    x.StatusChangingLastTime >= dateFrom &&
                    x.StatusChangingLastTime <= dateTo)
                .AsNoTracking()
                .AsSplitQuery();

            return await GetHistoryAsync(items,
            pageNumber,
            pageSize,
            itemStatus,
            sorting,
            dateFrom,
            dateTo,
            Filters.ItemTradingStatusesPurchasingFilterForChecking);
        }

        public async Task<MethodResult<PaginationViewModel<MyProfileItemTradingHistoryEntity>>> GetMyItemsHistoryAsync(Guid userId,
            int pageNumber,
            int pageSize,
            ItemTradingStatus itemStatus,
            SortingMethods sorting,
            DateTime dateFrom,
            DateTime dateTo)
        {
            var items = _catalogContext.ItemsTrading
                .Include(x => x.Images)
                .Where(x =>
                    x.UserId == userId && //owner, not customer
                    x.StatusChangingLastTime >= dateFrom &&
                    x.StatusChangingLastTime <= dateTo)
                .AsNoTracking()
                .AsSplitQuery();

            return await GetHistoryAsync(items,
            pageNumber,
            pageSize,
            itemStatus,
            sorting,
            dateFrom,
            dateTo,
            Filters.ItemTradingStatusesSellingFilterForChecking);
        }

        public async Task<MethodResult<PaginationViewModel<MyProfileItemTradingHistoryEntity>>> GetAuctionHistoryAsync(Guid userId,
            int pageNumber,
            int pageSize,
            ItemTradingStatus itemStatus,
            SortingMethods sorting,
            DateTime dateFrom,
            DateTime dateTo)
        {
            //переделать! история со слотов, тут надо подумать
            var items = _catalogContext.ItemsTrading
                .Include(i => i.Images)
                .Where(x =>
                    x.StatusChangingLastTime >= dateFrom &&
                    x.StatusChangingLastTime <= dateTo)
                .AsNoTracking()
                .AsSplitQuery();

            return await GetHistoryAsync(items,
            pageNumber,
            pageSize,
            itemStatus,
            sorting,
            dateFrom,
            dateTo,
            Filters.ItemTradingStatusesAuctionFilterForChecking);
        }


        private async Task<MethodResult<PaginationViewModel<MyProfileItemTradingHistoryEntity>>> GetHistoryAsync(IQueryable<ItemTrading> items,
            int pageNumber,
            int pageSize,
            ItemTradingStatus itemStatus,
            SortingMethods sorting,
            DateTime dateFrom,
            DateTime dateTo,
            IReadOnlyCollection<ItemTradingStatus> itemTradingStatusesForChecking)
        {
            var predicate = PredicateBuilder.New<ItemTrading>(false);//predicate for filtration

            //just only one allowed status
            if (itemTradingStatusesForChecking.Contains(itemStatus))
            {
                predicate = predicate.Or(x => x.Status == itemStatus);
            }

            //in case of AllStatuses (or another status not in allowed list  - return all items in allowed statuses (AllStatuses is not item status, just additional status)
            else
            {
                foreach (var statusFromList in itemTradingStatusesForChecking)
                {
                    if (statusFromList == ItemTradingStatus.AllStatuses)
                    {
                        continue;
                    }

                    predicate = predicate.Or(x => x.Status == statusFromList);
                }
            }

            items = items.Where(predicate);//filters item with predicate

            items = Sorting.SortItemsTrading(items, sorting);//sort items

            var totalItems = await items.CountAsync();

            var selectedItems = await items
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .Select(x => new MyProfileItemTradingHistoryEntity(x.Id, x.Title, x.StatusChangingLastTime, x.Images.First().SmallImagePath, $"статус ({x.Status.ToString()})")).ToListAsync(); //select items for pagination with projection to DTO

            return new(
             new PaginationViewModel<MyProfileItemTradingHistoryEntity>(
                selectedItems,
                pageNumber,
                totalItems,
                pageSize),
            [],
            MethodResults.Ok);
        }

        public async Task<MethodResult<PaginationViewModel<MyProfileReviewResponseEntity>>> GetProductsReviewsAsync(Guid currentUserId,
           int pageNumber,
           int pageSize,
           ReviewCommentsTypes reviewCommentType,
           SortingMethods sorting,
           DateTime dateFrom,
           DateTime dateTo)
        {
            var reviews = _catalogContext.Reviews
                .Where(x => x.CreationTime >= dateFrom && x.CreationTime <= dateTo)
                .AsNoTracking();

            Sorting.SortReviews(reviews, sorting);//sort items

            var predicate = PredicateBuilder.New<Review>(false);//predicate for filtration

            switch (reviewCommentType)
            {
                case ReviewCommentsTypes.My:
                    predicate = predicate.And(x => x.WriterId == currentUserId);
                    break;

                case ReviewCommentsTypes.OnMe:
                    predicate = predicate.And(x => x.WriterId != currentUserId);
                    break;
            }

            reviews = reviews.AsExpandable().Where(predicate);

            var total = await reviews.CountAsync();

            var selected = await reviews
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .Select(x => new MyProfileReviewResponseEntity(
                    x.Id,
                    x.WriterId,
                    _userManager.FindByIdAsync(x.WriterId.ToString()).GetAwaiter().GetResult()!.UserName!,
                    x.Body,
                    x.CreationTime,
                    currentUserId)).ToListAsync();

            return new(
             new PaginationViewModel<MyProfileReviewResponseEntity>(
                selected,
                pageNumber,
                total,
                pageSize),
            [],
            MethodResults.Ok);
        }

        public async Task<MethodResult<PaginationViewModel<MyProfileCommentResponseEntity>>> GetProductsCommentsAsync(Guid currentUserId,
           int pageNumber,
           int pageSize,
           ReviewCommentsTypes reviewCommentType,
           SortingMethods sorting,
           DateTime dateFrom,
           DateTime dateTo)
        {
            var comments = _catalogContext.Comments
                .Where(x => x.CreationTime >= dateFrom && x.CreationTime <= dateTo)
                .AsNoTracking();

            Sorting.SortComments(comments, sorting);//sort items

            var predicate = PredicateBuilder.New<Comment>(false);//predicate for filtration

            switch (reviewCommentType)
            {
                case ReviewCommentsTypes.My:
                    predicate = predicate.And(x => x.WriterId == currentUserId);
                    break;

                case ReviewCommentsTypes.OnMe:
                    predicate = predicate.And(x => x.WriterId != currentUserId);
                    break;
            }

            comments = comments.AsExpandable().Where(predicate);

            var total = await comments.CountAsync();

            var selected = await comments
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .Select(x => new MyProfileCommentResponseEntity(
                    x.Id,
                    x.WriterId,
                    _userManager.FindByIdAsync(x.WriterId.ToString()).GetAwaiter().GetResult()!.UserName!,
                    x.Body,
                    x.CreationTime,
                    currentUserId)).ToListAsync();

            return new(
             new PaginationViewModel<MyProfileCommentResponseEntity>(
                selected,
                pageNumber,
                total,
                pageSize),
            [],
            MethodResults.Ok);
        }

        public async Task<MethodResult<IEnumerable<MyProfileAddressDto>>> GetUserAddressesAsync(Guid userId)
        {
            var adresses = (await _userDbContext.Users
                .Include(u => u.Addresses)
                .AsNoTracking()
                .Where(x => x.Id == userId).ToListAsync())
                .SelectMany(u => u.Addresses.Select(a => new MyProfileAddressDto(
                    addressId: a.Id,
                    addressTitle: a.AddressTitle,
                    country: a.Country,
                    city: a.City,
                    region: a.Region,
                    district: a.District,
                    street: a.Street,
                    building: a.Building,
                    floor: a.Floor,
                    flat: a.Flat,
                    postIndex: a.PostIndex,
                    isForShipment: a.IsForShipment,
                    isDefaultForShipment: a.IsDefaultForShipment,
                    isForReceiving: a.IsForReceiving,
                    isDefaultForReceiving: a.IsDefaultForReceiving))
                );

            return new(adresses, [], MethodResults.Ok);
        }

        public async Task<MethodResult> CreateChangeAddressAsync(Guid userId, MyProfileAddressDto adressDto)
        {
            var user = await _userDbContext.Users.Include(u => u.Addresses).FirstOrDefaultAsync(x => x.Id == userId);

            if (user is null) { return new MethodResult([], MethodResults.Conflict); }

            var address = user.Addresses.FirstOrDefault(a => a.Id == adressDto.AddressId);

            if (address is null)
            {
                if (user.Addresses.FirstOrDefault(a => a.AddressTitle == adressDto.AddressTitle) is not null)
                {
                    return new MethodResult(["Адрес с таким названием уже существует"], MethodResults.Conflict);
                }

                address = new UserAddress(userId: userId,
                    adressDto.AddressTitle,
                    adressDto.Country,
                    adressDto.City,
                    adressDto.Region,
                    adressDto.District,
                    adressDto.Street,
                    adressDto.Building,
                    adressDto.Floor,
                    adressDto.Flat,
                    adressDto.PostIndex,
                    adressDto.IsForShipment,
                    adressDto.IsDefaultForShipment,
                    adressDto.IsForReceiving,
                    adressDto.IsDefaultForReceiving);

                user.Addresses.Add(address);
            }
            else
            {

                address.ChangeAddress(adressDto.AddressTitle,
                    adressDto.Country,
                    adressDto.City,
                    adressDto.Region,
                    adressDto.District,
                    adressDto.Street,
                    adressDto.Building,
                    adressDto.Floor,
                    adressDto.Flat,
                    adressDto.PostIndex,
                    adressDto.IsForShipment,
                    adressDto.IsDefaultForShipment,
                    adressDto.IsForReceiving,
                    adressDto.IsDefaultForReceiving);
            }

            await _userDbContext.SaveChangesAsync();

            return new MethodResult([], MethodResults.Ok);
        }

        public async Task<MethodResult> DeleteAddressAsync(Guid userId, Guid adressId)
        {
            var user = await _userDbContext.Users.Include(u => u.Addresses).FirstOrDefaultAsync(x => x.Id == userId);

            if (user is null) { return new MethodResult([], MethodResults.Conflict); }

            var adress = user.Addresses.FirstOrDefault(a => a.Id == adressId);

            if (adress is null)
            {
                return new MethodResult([], MethodResults.Ok);
            }

            user.Addresses.Remove(adress);

            await _userDbContext.SaveChangesAsync();

            return new MethodResult([], MethodResults.Ok);
        }

        public async Task<MethodResult<MyProfileItemTradingCreationChangingFilter>> GetFilterForItemCreationChangingAsync(Guid userId)
        {
            var addresses = await _userDbContext.Addresses
                .Where(x => x.UserId == userId && x.IsForShipment)
                .Select(a => new MyProfileAddressDto(
                    a.Id,
                    a.AddressTitle,
                    a.Country,
                    a.City,
                    a.Region,
                    a.District,
                    a.Street,
                    a.Building,
                    a.Floor,
                    a.Flat,
                    a.PostIndex,
                    a.IsForShipment,
                    a.IsDefaultForShipment,
                    a.IsForReceiving,
                    a.IsDefaultForReceiving))
                .AsNoTracking()
                .ToListAsync();

            /*var paymentMethods = await _userDbContext.Users
                .Include(u => u.PaymentMethods)
                .Where(x => x.Id == userId)
                .AsNoTracking()
                .SelectMany(x => x.PaymentMethods.Select(p => p.PaymentType))
            .ToListAsync();*/

            var paymentMethods = new List<Enums.PaymentMethod> { PaymentMethod.SafeDeal };

            var categories = await CreateCatalogCategoriesDtoAsync();

            if (categories is null) return new MethodResult<MyProfileItemTradingCreationChangingFilter>(null, ["Нет каталога"], MethodResults.Conflict);

            return new MethodResult<MyProfileItemTradingCreationChangingFilter>(
                new MyProfileItemTradingCreationChangingFilter(categories, addresses, paymentMethods), [], MethodResults.Ok);
        }

        private async Task<List<MyProfileCatalogCategory>?> CreateCatalogCategoriesDtoAsync()
        {
            List<MyProfileCatalogCategory> resCategories = [];

            var catalogCategories = await _catalogContext.CatalogCategories
                .Include(c => c.ChildrenCatalogCategories)
                .Include(c => c.CatalogIntPropertyNames)
                .Include(c => c.CatalogFloatPropertyNames)
                .Include(c => c.CatalogStringPropertyNames)
                    .ThenInclude(name => name.Properties)
                .Include(c => c.CatalogBoolPropertyNames)
                    .ThenInclude(name => name.Properties)
                .AsNoTracking()
                .ToListAsync();

            if (catalogCategories.Count == 0) return null; //нет категорий

            var zeroLevels = catalogCategories.Where(x => x.ParentCatalogCategory == null).ToList();

            if (zeroLevels.Count == 0) return null; //нет нулевого уровня

            foreach (var zeroLevel in zeroLevels)
            {
                var resCategory = new MyProfileCatalogCategory();
                resCategory.SystemName = zeroLevel.SystemName;
                resCategory.DisplayedName = zeroLevel.Name;

                resCategory.ChildrenCategories = CreateChildrenCatalogCategories(zeroLevel, resCategory);

                FillPropsToCatalogCategory(zeroLevel, resCategory);

                resCategories.Add(resCategory);
            }
            

            return resCategories;
        }

        private List<MyProfileCatalogCategory> CreateChildrenCatalogCategories(CatalogCategory parentCategory, MyProfileCatalogCategory parentCategoryDto)
        {
            List<MyProfileCatalogCategory> children = new();

            foreach(var category in parentCategory.ChildrenCatalogCategories)
            {
                var childCategory = new MyProfileCatalogCategory();
                childCategory.SystemName = category.SystemName;
                childCategory.DisplayedName = category.Name;

                childCategory.ChildrenCategories = CreateChildrenCatalogCategories(category, childCategory);

                FillPropsToCatalogCategory(category, childCategory);
                children.Add(childCategory);
            }

            return children;
        }

        private void FillPropsToCatalogCategory(CatalogCategory category, MyProfileCatalogCategory categoryDto)
        {

            categoryDto.IntPropertyNames = category.CatalogIntPropertyNames
                .Where(x => x.IsVisible)
                .Select(c => new MyProfileCatalogIntPropertyName { PropertyNameSystemName = c.SystemName, PropertyNameValue = c.Name})
                .ToList();

            categoryDto.FloatPropertyNames = category.CatalogFloatPropertyNames
                .Where(x => x.IsVisible)
                .Select(c => new MyProfileCatalogFloatPropertyName(c.SystemName, c.Name))
                .ToList();

            categoryDto.StringPropertyNames = category.CatalogStringPropertyNames
                .Where(x => x.IsVisible)
                .Select(c => new MyProfileCatalogStringPropertyName(
                    c.SystemName,
                    c.Name,
                    c.Properties
                    .Select(p => new MyProfileCatalogStringProperty(p.SystemValue, p.PropertyValue))
                    .OrderByDescending(p => p.Value)
                    .ToList()
                    )
                ).ToList();

            categoryDto.BoolPropertyNames = category.CatalogBoolPropertyNames
                .Where(x => x.IsVisible)
                .Select(c => new MyProfileCatalogBoolPropertyName(
                    c.SystemName,
                    c.Name,
                    c.Properties
                    .Select(p => new MyProfileCatalogBoolProperty(p.SystemValue, p.PropertyValue))
                    .OrderByDescending(p => p.Value)
                    .ToList()
                    )
                ).ToList();
        }
    }
}
