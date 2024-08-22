using Domain.Entities.Addresses;
using Domain.Entities.Images;
using Domain.Entities.Messages;
using Domain.Entities.Reviews;
using Domain.Entities.UserEntity;
using Domain.Interfaces.DbContexts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.UserDbContext
{
    public class UserDbContext : IdentityDbContext<User, UserRole, Guid>, IUserDbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
            //Database.Migrate();
        }

        public DbContext Instance => this;

        public DbSet<UserAddress> Addresses { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<ItemTradingReview> ItemTradingReviews { get; set; }

        public DbSet<ItemTradingReviewResponse> ItemTradingReviewsResponses { get; set; }

        public DbSet<ItemTradingReviewImage> ItemTradingReviewImages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.HasDefaultSchema("user_schema");
        }
        //EntityFrameworkCore\Add-Migration user_v_1 -Context UserDbContext
    }
}
