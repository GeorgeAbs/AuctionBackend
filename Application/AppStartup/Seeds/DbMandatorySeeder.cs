using Domain.CoreEnums;
using Domain.Entities.PaymentMethods;
using Domain.Entities.UserEntity;
using Domain.Interfaces.DbContexts;
using Domain.Interfaces.Services;
using Domain.Interfaces.Services.CatalogService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.AppStartup.Seeds
{
    public class DbMandatorySeeder
    {
        ICatalogDbContext _applicationContext;
        UserManager<User> _userManager;
        RoleManager<UserRole> _roleManager;
        private readonly ICatalogService _catalogService;
        private readonly IImageService _imageService;

        public DbMandatorySeeder(ICatalogDbContext applicationContext,
            UserManager<User> userManager,
            RoleManager<UserRole> roleManager,
            ICatalogService catalogService,
            IImageService imageService)
        {
            _applicationContext = applicationContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _catalogService = catalogService;
            _imageService = imageService;
        }

        public async Task SeedAsync()
        {
            //await CreatePaymentMethod(Enums.PaymentMethod.SafeDeal);
        }

        private async Task CreatePaymentMethod(Enums.PaymentMethod paymentType)
        {
            var existedMethod = await _applicationContext.PaymentMethods.FirstOrDefaultAsync(m => m.PaymentType == paymentType);
            if (existedMethod is not null) return;

            PaymentMethod paymentMethod = new (paymentType);

            _applicationContext.PaymentMethods.Add(paymentMethod);
            await _applicationContext.SaveChangesAsync();
        }
    }
}
