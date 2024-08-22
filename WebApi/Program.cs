using Application;
using Application.AppStartup;
using Application.AppStartup.Seeds;
using Application.Services;
using Application.Services.ItemTradingService;
using Application.Services.MyProfile;
using Domain.Entities.UserEntity;
using Domain.Interfaces;
using Domain.Interfaces.DbContexts;
using Domain.Interfaces.Services;
using Domain.Interfaces.Services.AdminGlobalSettingsService;
using Domain.Interfaces.Services.AuctionService;
using Domain.Interfaces.Services.BasketService;
using Domain.Interfaces.Services.CatalogPaginationService;
using Domain.Interfaces.Services.CatalogService;
using Domain.Interfaces.Services.ItemService.ItemTradingService;
using Domain.Interfaces.Services.ModerationService;
using Domain.Interfaces.Services.MyProfileService;
using Domain.Interfaces.Services.Orders;
using Infrastructure.Data.CatalogDbContext;
using Infrastructure.Data.EntitiesCountersDbContext;
using Infrastructure.Data.UserDbContext;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Globalization;
using System.Text;
using WebApi.Constants;
using WebApi.Hubs;
using WebApi.Interfaces;
using WebApi.Middlewares;
using WebApi.Services;
using WebApi.SwaggerFilters;

namespace WebApi
{
    public class Program
    {
         public static async Task Main(string[] args)
         {
            //StartupFoldersCreator.Generate();

            const string AUCTION_CORS_POLICY = "auction";

            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddStackExchangeRedisCache(options => {
                options.Configuration = "localhost";
                options.InstanceName = "local";
            });

            builder.Services.AddSignalR();


            builder.Services.AddDbContext<ICatalogDbContext, CatalogDbContext>(c =>
                c.UseNpgsql(builder.Configuration.GetConnectionString("ApplicationConnection")));

            builder.Services.AddDbContext<IUserDbContext, UserDbContext>(c =>
                c.UseNpgsql(builder.Configuration.GetConnectionString("ApplicationConnection")));

            builder.Services.AddDbContext<IEntitiesCountersContext, EntitiesCountersDbContext>(c =>
                c.UseNpgsql(builder.Configuration.GetConnectionString("ApplicationConnection")));

            builder.Services.AddIdentity<User, UserRole>(opt =>
            {
                opt.Password.RequiredLength = 7;
                opt.Password.RequireDigit = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequiredUniqueChars = 2;
                opt.User.AllowedUserNameCharacters += ' ';
                //opt.User.RequireUniqueEmail = true; // сравнивает с "" !!
            })
            .AddEntityFrameworkStores<UserDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromMinutes(50));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // указывает, будет ли валидироваться издатель при валидации токена
                    ValidateIssuer = true,
                    // строка, представляющая издателя
                    ValidIssuer = "3d",
                    // будет ли валидироваться потребитель токена
                    ValidateAudience = true,
                    // установка потребителя токена
                    ValidAudience = "3d",
                    // будет ли валидироваться время существования
                    ValidateLifetime = true,
                    // установка ключа безопасности
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("990d1b7f-434a-426a-bd1a-ad4877c18fc4")),//hide to separate file!
                                                                                                                                // валидация ключа безопасности
                    ValidateIssuerSigningKey = true,
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // если запрос направлен хабу
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments(HubsPaths.TEST_HUB_PATH) || path.StartsWithSegments(HubsPaths.BID_HUB_PATH)))
                        {
                            // получаем токен из строки запроса
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddControllers();


            builder.Services.AddScoped(typeof(IHttpUserService), typeof(HttpUserService));
            builder.Services.AddScoped(typeof(IItemTradingService<>), typeof(ItemTradingService<>));
            builder.Services.AddScoped(typeof(ITokenService), typeof(IdentityTokenService));
            builder.Services.AddScoped(typeof(IAuthService), typeof(AuthService));
            builder.Services.AddScoped(typeof(IUserService<,>), typeof(UserService<,>));
            builder.Services.AddScoped(typeof(IAutoModerator), typeof(AutoModeratorService));
            builder.Services.AddScoped(typeof(ICatalogService), typeof(CatalogService));
            builder.Services.AddScoped(typeof(IModerationService<>), typeof(ItemTradingModerationService<>));
            builder.Services.AddScoped(typeof(IBasketService), typeof(BasketService));
            builder.Services.AddScoped(typeof(IMyProfileService), typeof(MyProfileService));
            builder.Services.AddScoped(typeof(IUserNotifier), typeof(UserNotifier));
            builder.Services.AddScoped(typeof(IAuctionEndingService), typeof(AuctionEndingService));
            builder.Services.AddScoped(typeof(ICatalogPaginationService), typeof(CatalogPaginationService));
            builder.Services.AddScoped(typeof(IOrderCreationService), typeof(OrderCreationService));
            builder.Services.AddScoped(typeof(IIdentifiersService), typeof(IdentifiersService));
            builder.Services.AddScoped(typeof(IAdminGlobalSettingsService), typeof(AdminGlobalSettingsService));
            builder.Services.AddScoped(typeof(IBidsService), typeof(BidsService));
            builder.Services.AddSingleton(typeof(ILocalizer), typeof(Localizer));
            builder.Services.AddSingleton(typeof(IImageService), typeof(ImageService));
            builder.Services.AddSingleton<IEmailSender, EmailSender>();

            builder.Services.AddHostedService<AuctionBackgroundService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.AddSignalRSwaggerGen();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                        new OpenApiSecurityScheme
                        {
                        Reference = new OpenApiReference
                        {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                        }
            },
            new string[]{}
        }
    });
                //c.OperationFilter<ItemTradingAuctionSlotInfoRequestFilter>(); not required now
            });

            builder.Services.AddCors(option => option.AddPolicy(AUCTION_CORS_POLICY, policy =>
            {
                policy
                //.AllowAnyOrigin()
                .WithOrigins(
                    
                    "https://localhost:81",
                    "https://localhost:4200",
                    "https://127.0.0.1:4200",
                    "https://127.0.0.1:81",
                    "https://127.0.0.1:7152",
                    "https://127.0.0.1:5173",
                    "http://127.0.0.1:5173",
                    "https://localhost:5173",
                    "http://localhost:5173",
                    "https://localhost:7152")
                .AllowCredentials()
                .AllowAnyHeader()
                .AllowAnyMethod();
            }));

            var app = builder.Build();

            app.UseMiddleware<GlobalRoutePrefixMiddleware>("/api");
            app.UsePathBase(new PathString("/api"));

            app.UseCors("auction");

            var ci = CultureInfo.InvariantCulture;
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(ci),
                SupportedCultures = new List<CultureInfo>
                {
                    ci
                },
                SupportedUICultures = new List<CultureInfo>
                {
                    ci
                }
            });

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var appContext = services.GetRequiredService<CatalogDbContext>();
                appContext.Database.Migrate();

                var userContext = services.GetRequiredService<UserDbContext>();
                userContext.Database.Migrate();

                var entitiesCountersDbContext = services.GetRequiredService<EntitiesCountersDbContext>();
                entitiesCountersDbContext.Database.Migrate();
            }

            for (int i = 0; i < 1; i++)
            {
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    var cache = services.GetRequiredService<IDistributedCache>();
                    var appContext = services.GetRequiredService<CatalogDbContext>();//should be the first for testing
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var userContext = services.GetRequiredService<UserDbContext>();
                    var roleManager = services.GetRequiredService<RoleManager<UserRole>>();
                    var catalogService = services.GetRequiredService<ICatalogService>();
                    var imageService = services.GetRequiredService<IImageService>();
                    var userDbContext = services.GetRequiredService<IUserDbContext>();

                    try
                    {
                        DbSeeder dbSeeder = new(appContext, userManager, roleManager, catalogService, imageService, userDbContext);
                        await dbSeeder.SeedAsync();
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred while seeding the database.");
                    }

                    finally
                    {

                    }
                }
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var cache = services.GetRequiredService<IDistributedCache>();
                var appContext = services.GetRequiredService<CatalogDbContext>();
                var paginationService = services.GetRequiredService<ICatalogPaginationService>();
                try
                {
                    StartupCacheGenerator startupCacheGenerator = new(appContext, cache, paginationService);
                    await startupCacheGenerator.GenerateCacheAsync();
                }
                catch(Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while initial caching.");
                }
                
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var cache = services.GetRequiredService<IDistributedCache>();
                var appContext = services.GetRequiredService<CatalogDbContext>();//should be the first for testing
                var userManager = services.GetRequiredService<UserManager<User>>();
                var userContext = services.GetRequiredService<UserDbContext>();
                var roleManager = services.GetRequiredService<RoleManager<UserRole>>();
                var catalogService = services.GetRequiredService<ICatalogService>();
                var imageService = services.GetRequiredService<IImageService>();
                try
                {
                    DbMandatorySeeder dbMandatorySeeder = new(appContext, userManager, roleManager, catalogService, imageService);
                    await dbMandatorySeeder.SeedAsync();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while initial seeding.");
                }

            }


            if (app.Environment.IsDevelopment())
            {
                //app.UseSwagger(c =>
                //{
                //    c.PreSerializeFilters.Add((swagger, httpReq) =>
                //    {
                //        swagger.Servers.Clear();
                //        swagger.Servers.Add(new OpenApiServer { Url = @"*" });
                //    });
                //});
                app.UseSwagger();
                app.UseSwaggerUI();

                
            }

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider($"{Settings.RootFilesPath}"),
                RequestPath = new PathString(Settings.FilesRootDir)
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider($"{Settings.RootFilesPath}"),

                RequestPath = new PathString("/files")
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.MapHub<BidHub>(HubsPaths.BID_HUB_PATH);
            app.MapHub<UserHub>(HubsPaths.USER_HUB_PATH);
            app.MapHub<ChatHub>(HubsPaths.CHAT_HUB_PATH);
            app.Run();
        }
    }
}