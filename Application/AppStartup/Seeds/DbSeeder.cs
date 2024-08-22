using Domain.Constants;
using Domain.CoreEnums;
using Domain.Entities.Addresses;
using Domain.Entities.AuctionSlots;
using Domain.Entities.Catalog;
using Domain.Entities.Catalog.CatalogProperty;
using Domain.Entities.Catalog.CatalogProperty.CatalogPropertiesNames;
using Domain.Entities.Images;
using Domain.Entities.Items.ItemTrading;
using Domain.Entities.Messages;
using Domain.Entities.UserEntity;
using Domain.Interfaces.DbContexts;
using Domain.Interfaces.Services;
using Domain.Interfaces.Services.CatalogService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;

namespace Application.AppStartup.Seeds
{
    public class DbSeeder
    {
        ICatalogDbContext _applicationContext;
        IUserDbContext _userDbContext;
        UserManager<User> _userManager;
        RoleManager<UserRole> _roleManager;
        private readonly ICatalogService _catalogService;
        private readonly IImageService _imageService;

        public DbSeeder(ICatalogDbContext applicationContext,
            UserManager<User> userManager,
            RoleManager<UserRole> roleManager,
            ICatalogService catalogService,
            IImageService imageService,
            IUserDbContext userDbContext)
        {
            _applicationContext = applicationContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _catalogService = catalogService;
            _imageService = imageService;
            _userDbContext = userDbContext;
        }
        async public Task SeedAsync()
        {
            var userAdmin = await CreateRole(RoleConstants.ADMIN);
            

            var user = await CreateRole(RoleConstants.DEFAULT_USER);
            

            var superUser = await CreateRole(RoleConstants.SUPER_USER);

            var moderatorUser = await CreateRole(RoleConstants.MODERATOR);

            if (user is not null) await AddAddressesAsync(user);

            if (superUser is not null) await AddAddressesAsync(superUser);

            if (moderatorUser is not null) await AddAddressesAsync(moderatorUser);

            if (userAdmin is not null)
            {
                await AddAddressesAsync(userAdmin);
                await _userManager.AddToRoleAsync(userAdmin, RoleConstants.MODERATOR);
                await _userManager.AddToRoleAsync(userAdmin, RoleConstants.DEFAULT_USER);
            }

            await CreateCatalogAsync();

            await CreateItems();

            await CreateBannerAsync();

            GC.Collect();
        }

        private async Task CreateBannerAsync()
        {
            var existedBanner = await _applicationContext.BannerImages.CountAsync();
            if (existedBanner > 0) return;

            Stream image = File.OpenRead((new DirectoryInfo($"{Directory.GetCurrentDirectory()}/wwwroot/images/first_page_banner")).GetFiles().First().FullName);


            image.Position = 0;

            using Stream bigStream = new MemoryStream();
            using Stream smallStream = new MemoryStream();
            image.CopyTo(bigStream);
            image.Position = 0;
            image.CopyTo(smallStream);
            bigStream.Position = 0;
            smallStream.Position = 0;


            var bigImage = await _imageService.SaveImageAsync(Domain.CoreEnums.Enums.ImagePurpose.BigImage, bigStream);
            var smallImage = await _imageService.SaveImageAsync(Domain.CoreEnums.Enums.ImagePurpose.SmallImage, smallStream);

            BannerImage bannerImage = new BannerImage(bigImage.ResultEntity!, smallImage.ResultEntity!, Enums.BannerType.FistPageBanner);

            _applicationContext.BannerImages.Add(bannerImage);
            await _applicationContext.SaveChangesAsync();
        }

        async Task<User?> CreateRole(string role)
        {

            if (await _roleManager.FindByNameAsync(role) == null)
            {
                await _roleManager.CreateAsync(new UserRole(role));

                var randomInt = Random.Shared.Next(10000);

                User newUser = new (
                    $"userName {randomInt}",
                    DateTime.UtcNow,
                    $"{role}@com.com",
                    "12345",
                    "12345");
                newUser.IsEmailActivated = true;
                
                List<Stream> images = new();
                foreach (var file in new DirectoryInfo($"{Directory.GetCurrentDirectory()}/wwwroot/images").GetFiles())
                {
                    images.Add(File.OpenRead(file.FullName));
                }

                var pic = images[Random.Shared.Next(images.Count)];
                pic.Position = 0;

                using Stream bigStream = new MemoryStream();
                using Stream smallStream = new MemoryStream();
                pic.CopyTo(bigStream);
                pic.Position = 0;
                pic.CopyTo(smallStream);
                bigStream.Position = 0;
                smallStream.Position = 0;


                var bigImage = await _imageService.SaveImageAsync(Domain.CoreEnums.Enums.ImagePurpose.BigImage, bigStream);
                var smallImage = await _imageService.SaveImageAsync(Domain.CoreEnums.Enums.ImagePurpose.SmallImage, smallStream);

                newUser.ChangeShopLogo(bigImage.ResultEntity!, smallImage.ResultEntity!);
                newUser.ChangeUserLogo(bigImage.ResultEntity!, smallImage.ResultEntity!);

                var res = await _userManager.CreateAsync(newUser, "!Ab12345");
                await _userManager.SetEmailAsync(newUser, newUser.Email);
                foreach (var item in res.Errors)
                {
                    Console.WriteLine(item.Description);
                }

                await _userManager.AddToRoleAsync(newUser, role);

                return newUser;
            }

            return null;
        }

        private async Task AddAddressesAsync(User user)
        {
            var addressesCount = await _userDbContext.Addresses.CountAsync(a => a.UserId == user.Id);
            if (addressesCount > 0) return;

            for (int i = 0; i < 2; i++) 
            {
                bool isForShipment = i == 0 ? true : false;
                bool isDefaultForShipment = isForShipment ? true : false;
                bool isForReceiving = i == 1 ? true : false;
                bool isDefaultForReceiving = isForReceiving ? true : false;

                UserAddress address = new UserAddress(user.Id,
                $"title {i}",
                $"country {i}",
                $"city {i}",
                $"region {i}",
                $"district {i}",
                $"street {i}",
                $"building {i}",
                $"floor {i}",
                $"flat {i}",
                $"postIndex {i}",
                isForShipment,
                isDefaultForShipment,
                isForReceiving,
                isDefaultForReceiving);

                _userDbContext.Addresses.Add(address);
            }

           await _userDbContext.SaveChangesAsync();
        }

        async Task CreateCatalogAsync()
        {
            if (_applicationContext.CatalogCategories.Any()) return;

            var jsonCategoriesText = File.ReadAllText(Path.GetFullPath("wwwroot/SeedCatalogCategories.json"));

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
            };

            var jsonCategories = JsonSerializer.Deserialize<SeedCategories>(jsonCategoriesText, options);

            foreach (var category in jsonCategories.Categories)
            {
                var newCategory = CreateCategory(category);

                foreach(var name in category.StringPropNames)
                {
                    var newName = CreateStringPropName(newCategory, name);
                    newCategory.AddCatalogPropertyName(newName);
                }

                foreach (var name in category.BoolPropNames)
                {
                    var newName = CreateBoolPropName(newCategory, name);
                    newCategory.AddCatalogPropertyName(newName);
                }

                foreach (var name in category.FloatPropNames)
                {
                    var newName = CreateFloatPropName(newCategory, name);
                    newCategory.AddCatalogPropertyName(newName);
                }

                foreach (var name in category.IntPropNames)
                {
                    var newName = CreateIntPropName(newCategory, name);
                    newCategory.AddCatalogPropertyName(newName);
                }
                _applicationContext.CatalogCategories.Add(newCategory);


            }

            await _applicationContext.SaveChangesAsync();
        }

        private async Task CreateItems()
        {
            var itemsCount = await _applicationContext.ItemsTrading.CountAsync();

            if (itemsCount > 1) return;

            List<Stream> images = new();

            foreach (var file in new DirectoryInfo($"{Directory.GetCurrentDirectory()}/wwwroot/images").GetFiles())
            {
                images.Add(File.OpenRead(file.FullName));
            }

            var users = _userManager.Users.ToList();
            var usersCount = users.Count;

            var categories = _applicationContext.CatalogCategories
                .Include(c => c.CatalogStringPropertyNames)
                    .ThenInclude(n => n.Properties)
                .Include(c => c.CatalogBoolPropertyNames)
                    .ThenInclude(n => n.Properties)
                .Include(c => c.CatalogFloatPropertyNames)
                .Include(c => c.CatalogIntPropertyNames)
                .Where(c => c.CatalogStringPropertyNames.Any()).ToList();

            var categoriesCount = categories.Count;

            List<ItemTrading> items = [];
            List<ItemTradingAuctionSlot> newSlots = [];

            for (int ii = 0; ii < categoriesCount; ii++)
            {
                var category = categories[ii];
                for (int i = 0; i < 30; i++)
                {
                    ItemTrading item;

                    var user = users[Random.Shared.Next(usersCount)];

                    var address = await _userDbContext.Addresses.FirstAsync(a => a.UserId == user.Id && a.IsForShipment);

                    List<ItemTradingAuctionSlot> itemSlots = [];

                    if (Random.Shared.Next(0,20) > 10)
                    {
                        item = new ItemTrading(category, user.Id, $"title {Random.Shared.Next()}", $"description {Random.Shared.Next()}", Domain.CoreEnums.Enums.SellingTypes.Standard);
                    }

                    else
                    {
                        item = new ItemTrading(category, user.Id, $"title {Random.Shared.Next()}", $"description {Random.Shared.Next()}", Domain.CoreEnums.Enums.SellingTypes.Auction);

                        for (int iSlot = 0; iSlot < 6; iSlot++)
                        {
                            var newSlot = new ItemTradingAuctionSlot(category,
                                item.Id,
                                "Title",
                                "Description",
                                (float)Random.Shared.Next(100, 200),
                                (float)Random.Shared.Next(100, 200),
                                (float)Random.Shared.Next(500, 1000),
                                DateTime.UtcNow.AddDays(70),
                                iSlot + 1);

                            itemSlots.Add(newSlot);

                            var pic = images[Random.Shared.Next(images.Count)];
                            pic.Position = 0;

                            using Stream bigStream = new MemoryStream();
                            using Stream smallStream = new MemoryStream();
                            pic.CopyTo(bigStream);
                            pic.Position = 0;
                            pic.CopyTo(smallStream);
                            bigStream.Position = 0;
                            smallStream.Position = 0;

                            var bigImage = await _imageService.SaveImageAsync(Domain.CoreEnums.Enums.ImagePurpose.BigImage, bigStream);
                            //await Console.Out.WriteLineAsync("image big ok");
                            var smallImage = await _imageService.SaveImageAsync(Domain.CoreEnums.Enums.ImagePurpose.SmallImage, smallStream);
                            //await Console.Out.WriteLineAsync("image small ok");
                            var newImage = new ItemTradingSlotImage(newSlot, bigImage.ResultEntity, smallImage.ResultEntity);

                            _applicationContext.ItemTradingAuctionSlots.Add(newSlot);
                            _applicationContext.ItemsTradingsSlotsImages.Add(newImage);

                            List<CatalogStringProperty> stringPropertiesSlot = [];
                            List<CatalogBoolProperty> boolPropertiesSlot = [];
                            List<CatalogFloatProperty> floatPropertiesSlot = [];
                            List<CatalogIntProperty> intPropertiesSlot = [];

                            foreach (var c in category.CatalogStringPropertyNames)
                            {
                                var propCount = c.Properties.Count();
                                stringPropertiesSlot.Add(c.Properties.ElementAt(Random.Shared.Next(propCount)));
                            }

                            foreach (var c in category.CatalogBoolPropertyNames)
                            {
                                var propCount = c.Properties.Count();
                                boolPropertiesSlot.Add(c.Properties.ElementAt(Random.Shared.Next(propCount)));
                            }

                            foreach (var c in category.CatalogFloatPropertyNames)
                            {
                                var prop = new CatalogFloatProperty(Random.Shared.NextSingle() * 1000f, item, c);
                                c.AddProperty(prop);
                                floatPropertiesSlot.Add(prop);
                            }

                            foreach (var c in category.CatalogIntPropertyNames)
                            {
                                var prop = new CatalogIntProperty(Random.Shared.Next(100), item, c);
                                c.AddProperty(prop);
                                intPropertiesSlot.Add(prop);
                            }

                            newSlot.SetInfo("Description",
                                (float)Random.Shared.Next(100, 200),
                                (float)Random.Shared.Next(100, 200),
                                (float)Random.Shared.Next(500, 1000),
                                DateTime.UtcNow.AddDays(70),
                                iSlot + 1,
                                [newImage]/*currently we have only 1 image per slot*/,
                                stringPropertiesSlot, floatPropertiesSlot, intPropertiesSlot, boolPropertiesSlot);

                            newSlots.Add(newSlot);
                        }
                    }

                    var random = Random.Shared.Next(0, 20);

                    if (random > 5)
                    {
                        item.ChangeItemStatus(Domain.CoreEnums.Enums.ItemTradingStatus.Moderation);
                        item.ChangeItemStatus(Domain.CoreEnums.Enums.ItemTradingStatus.Published);

                        if (item.SellingType == Enums.SellingTypes.Auction)
                        {
                            foreach (var slot in itemSlots)
                            {
                                slot.ChangeStatus(Enums.AuctionSlotStatus.Started);
                            }
                        }
                    }

                    else if (random > 1 && random <= 5)
                    {
                        item.ChangeItemStatus(Domain.CoreEnums.Enums.ItemTradingStatus.Moderation);
                    }

                    else if (random <= 1)
                    {
                        item.ChangeItemStatus(Domain.CoreEnums.Enums.ItemTradingStatus.Moderation);

                        _applicationContext.ItemTradingModerationDisappReasons.Add(new(item, "Выглядит сомнительно", user.Id));
                        item.ChangeItemStatus(Domain.CoreEnums.Enums.ItemTradingStatus.DisapprovedByModerator);
                    }


                    item.SetPromotionByDesign(Enums.DesignPromotionType.FirstType);
                    item.EnablePromotionByPriority(true);

                    items.Add(item);

                    List<ItemTradingImage> newImages = []; 

                    for(int iIm = 0; iIm < 2; iIm++)
                    {
                        var pic = images[Random.Shared.Next(images.Count)];
                        pic.Position = 0;

                        using Stream bigStream = new MemoryStream();
                        using Stream smallStream = new MemoryStream();
                        pic.CopyTo(bigStream);
                        pic.Position = 0;
                        pic.CopyTo(smallStream);
                        bigStream.Position = 0;
                        smallStream.Position = 0;


                        var bigImage = await _imageService.SaveImageAsync(Domain.CoreEnums.Enums.ImagePurpose.BigImage, bigStream);
                        //await Console.Out.WriteLineAsync("image big ok");
                        var smallImage = await _imageService.SaveImageAsync(Domain.CoreEnums.Enums.ImagePurpose.SmallImage, smallStream);
                        //await Console.Out.WriteLineAsync("image small ok");
                        var newImage = new ItemTradingImage(item, bigImage.ResultEntity, smallImage.ResultEntity);

                        newImages.Add(newImage);
                    }

                    if (item.SellingType == Enums.SellingTypes.Standard)
                    {
                        List<CatalogStringProperty> stringProperties = [];
                        List<CatalogBoolProperty> boolProperties = [];
                        List<CatalogFloatProperty> floatProperties = [];
                        List<CatalogIntProperty> intProperties = [];

                        foreach (var c in category.CatalogStringPropertyNames)
                        {
                            var propCount = c.Properties.Count();
                            stringProperties.Add(c.Properties.ElementAt(Random.Shared.Next(propCount)));
                        }

                        foreach (var c in category.CatalogBoolPropertyNames)
                        {
                            var propCount = c.Properties.Count();
                            boolProperties.Add(c.Properties.ElementAt(Random.Shared.Next(propCount)));
                        }

                        foreach (var c in category.CatalogFloatPropertyNames)
                        {
                            var prop = new CatalogFloatProperty(Random.Shared.NextSingle() * 1000f, item, c);
                            c.AddProperty(prop);
                            floatProperties.Add(prop);
                        }

                        foreach (var c in category.CatalogIntPropertyNames)
                        {
                            var prop = new CatalogIntProperty(Random.Shared.Next(100), item, c);
                            c.AddProperty(prop);
                            intProperties.Add(prop);
                        }

                        item.SetSimpleItemInfo($"title {Random.Shared.Next()}",
                            $"description {Random.Shared.Next()}",
                            Random.Shared.Next(500, 5001),
                            stringProperties,
                            intProperties,
                            floatProperties,
                            boolProperties,
                            [new ItemAddress(item, address.AddressTitle, address.Country, address.City, address.Region, address.District, address.Street, address.Building, address.Floor, address.Flat, address.PostIndex)],
                            [Enums.PaymentMethod.SafeDeal],
                            newImages);
                    }

                    else
                    {
                        //collect all props from all slots
                        var allStringProps = newSlots.SelectMany(x => x.StringProperties).DistinctBy(x => x.SystemValue).ToList();
                        var allBoolProps = newSlots.SelectMany(x => x.BoolProperties).DistinctBy(x => x.SystemValue).ToList();
                        var allIntProps = newSlots.SelectMany(x => x.IntProperties).DistinctBy(x => x.PropertyValue).ToList();
                        var allFloatProps = newSlots.SelectMany(x => x.FloatProperties).DistinctBy(x => x.PropertyValue).ToList();

                        //find max and min start price
                        var minPrice = newSlots.Select(x => x.Price).Min();
                        var maxPrice = newSlots.Select(x => x.Price).Max();

                        item.SetAuctionItemInfo($"title {Random.Shared.Next()}",
                            $"description {Random.Shared.Next()}",
                            minPrice,
                            maxPrice,
                            allStringProps,
                            allIntProps,
                            allFloatProps,
                            allBoolProps,
                            [new ItemAddress(item, address.AddressTitle, address.Country, address.City, address.Region, address.District, address.Street, address.Building, address.Floor, address.Flat, address.PostIndex)],
                            [Enums.PaymentMethod.SafeDeal],
                            newImages,
                             DateTime.UtcNow.AddDays(70));
                    }

                    var question = new ItemTradingQuestion(item, user.Id, "Это хороший товар?");
                    var answer = new ItemTradingAnswer(item, item.UserId, "Самый лучший на диком западе!", question);

                    var question2 = new ItemTradingQuestion(item, user.Id, "Скидочка будет? Сразу куплю");
                    var answer2 = new ItemTradingAnswer(item, item.UserId, "Ага, конечно...", question2);

                    var review = new ItemTradingReview(user, user.Id, "В целом парень неплохой, только ссытся и кривой", item.Id, Random.Shared.Next(1, 6), item.Title);
                    var reviewAnswer = new ItemTradingReviewResponse(user, user.Id, "Ну как же так!", review);

                    for(int iPic = 0; iPic < Random.Shared.Next(0,4); iPic++)
                    {
                        var pic = images[Random.Shared.Next(images.Count)];
                        pic.Position = 0;

                        using Stream bigStream = new MemoryStream();
                        using Stream smallStream = new MemoryStream();
                        pic.CopyTo(bigStream);
                        pic.Position = 0;
                        pic.CopyTo(smallStream);
                        bigStream.Position = 0;
                        smallStream.Position = 0;


                        var bigImage = await _imageService.SaveImageAsync(Domain.CoreEnums.Enums.ImagePurpose.BigImage, bigStream);
                        //await Console.Out.WriteLineAsync("image big ok");
                        var smallImage = await _imageService.SaveImageAsync(Domain.CoreEnums.Enums.ImagePurpose.SmallImage, smallStream);
                        //await Console.Out.WriteLineAsync("image small ok");
                        var reviewImage = new ItemTradingReviewImage(review, bigImage.ResultEntity!, smallImage.ResultEntity!);

                        _userDbContext.ItemTradingReviewImages.Add(reviewImage);
                    }
                    

                    

                    var review2 = new ItemTradingReview(user, user.Id, "Доехало в целости", item.Id, Random.Shared.Next(1, 6), item.Title);
                    var reviewAnswer2 = new ItemTradingReviewResponse(user, user.Id, "Пупырки много не бывает :)", review2);

                    _applicationContext.ItemTradingQuestions.Add(question);
                    _applicationContext.ItemTradingQuestions.Add(question2);
                    _applicationContext.ItemTradingAnswers.Add(answer);
                    _applicationContext.ItemTradingAnswers.Add(answer2);

                    _userDbContext.ItemTradingReviews.Add(review);
                    _userDbContext.ItemTradingReviews.Add(review2);
                    _userDbContext.ItemTradingReviewsResponses.Add(reviewAnswer);
                    _userDbContext.ItemTradingReviewsResponses.Add(reviewAnswer2);
                }
            }
            

            _applicationContext.ItemsTrading.AddRange(items);
            

            foreach (var image in images)
            {
                image.Dispose();
            }

            await _applicationContext.SaveChangesAsync();
            await _userDbContext.SaveChangesAsync();
        }

        private CatalogCategory CreateCategory(SeedCategory seedCategory, CatalogCategory? parentCategory = null)
        {
            var newParentCategory = new CatalogCategory(seedCategory.Name, seedCategory.SystemName, parentCategory);

            foreach (var name in seedCategory.StringPropNames)
            {
                var newName = CreateStringPropName(newParentCategory, name);
                newParentCategory.AddCatalogPropertyName(newName);
            }

            foreach (var name in seedCategory.BoolPropNames)
            {
                var newName = CreateBoolPropName(newParentCategory, name);
                newParentCategory.AddCatalogPropertyName(newName);
            }

            foreach (var name in seedCategory.FloatPropNames)
            {
                var newName = CreateFloatPropName(newParentCategory, name);
                newParentCategory.AddCatalogPropertyName(newName);
            }

            foreach (var name in seedCategory.IntPropNames)
            {
                var newName = CreateIntPropName(newParentCategory, name);
                newParentCategory.AddCatalogPropertyName(newName);
            }

            parentCategory?.AddChildrenCategory(newParentCategory);

            foreach (var cat in seedCategory.ChildrenCategories)
            {

                CreateCategory(cat, newParentCategory);
            }
            return newParentCategory;
        }

        private CatalogStringPropertyName CreateStringPropName(CatalogCategory catalogCategory, SeedStringName propName)
        {
            var newName = new CatalogStringPropertyName(catalogCategory, propName.Name, propName.SystemName);

            foreach (var prop in propName.Props)
            {
                var newProp = new CatalogStringProperty(prop.Value, newName, prop.SystemValue);
                newName.AddProperty(newProp);
            }

            return newName;
        }

        private CatalogBoolPropertyName CreateBoolPropName(CatalogCategory catalogCategory, SeedBoolName propName)
        {
            var newName = new CatalogBoolPropertyName(catalogCategory, propName.Name, propName.SystemName);

            foreach (var prop in propName.Props)
            {
                var newProp = new CatalogBoolProperty(prop.Value, newName, prop.SystemValue);
                newName.AddProperty(newProp);
            }

            return newName;
        }

        private CatalogFloatPropertyName CreateFloatPropName(CatalogCategory catalogCategory, SeedFloatName propName)
        {
            var newName = new CatalogFloatPropertyName(catalogCategory, propName.Name, propName.SystemName);

            return newName;
        }

        private CatalogIntPropertyName CreateIntPropName(CatalogCategory catalogCategory, SeedIntName propName)
        {
            var newName = new CatalogIntPropertyName(catalogCategory, propName.Name, propName.SystemName);

            return newName;
        }

    }
    #region
    public class SeedCategories
    {
        public IEnumerable<SeedCategory> Categories { get; set; } = [];

        public SeedCategories() { }
    }

    public class SeedCategory
    {
        public string Name { get; set; }

        public string SystemName { get; set; }

        public IEnumerable<SeedCategory> ChildrenCategories { get; set; } = [];

        public IEnumerable<SeedStringName> StringPropNames { get; set; } = [];

        public IEnumerable<SeedBoolName> BoolPropNames { get; set; } = [];

        public IEnumerable<SeedFloatName> FloatPropNames { get; set; } = [];

        public IEnumerable<SeedIntName> IntPropNames { get; set; } = [];

        public SeedCategory() { }
    }

    public class SeedStringName
    {
        public string Name { get; set; }

        public string SystemName { get; set; }

        public IEnumerable<SeedStringProp> Props { get; set; } = [];

        public SeedStringName() { }
    }

    public class SeedBoolName
    {
        public string Name { get; set; }

        public string SystemName { get; set; }

        public IEnumerable<SeedBoolProp> Props { get; set; } = [];

        public SeedBoolName() { }
    }

    public class SeedFloatName
    {
        public string Name { get; set; }

        public string SystemName { get; set; }
    }

    public class SeedIntName
    {
        public string Name { get; set; }

        public string SystemName { get; set; }

        public SeedIntName() { }
    }

    public class SeedStringProp
    {
        public string Value { get; set; }

        public string SystemValue { get; set; }

        public SeedStringProp() { }
    }

    public class SeedBoolProp
    {
        public string Value { get; set; }

        public string SystemValue { get; set; }

        public SeedBoolProp() { }
    }
    #endregion
}
