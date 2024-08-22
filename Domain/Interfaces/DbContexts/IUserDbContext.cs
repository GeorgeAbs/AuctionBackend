using Domain.Entities.Addresses;
using Domain.Entities.Images;
using Domain.Entities.Messages;
using Domain.Entities.Reviews;
using Domain.Entities.UserEntity;
using Microsoft.EntityFrameworkCore;

namespace Domain.Interfaces.DbContexts
{
    public interface IUserDbContext : IDbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<UserAddress> Addresses { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<ItemTradingReview> ItemTradingReviews { get; set; }

        public DbSet<ItemTradingReviewResponse> ItemTradingReviewsResponses { get; set; }

        public DbSet<ItemTradingReviewImage> ItemTradingReviewImages { get; set; }
    }
}
