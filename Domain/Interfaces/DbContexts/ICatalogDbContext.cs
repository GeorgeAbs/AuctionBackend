using Domain.Entities.Addresses;
using Domain.Entities.AuctionSlots;
using Domain.Entities.Basket;
using Domain.Entities.Bid;
using Domain.Entities.Catalog;
using Domain.Entities.Catalog.CatalogProperty;
using Domain.Entities.Catalog.CatalogProperty.CatalogPropertiesNames;
using Domain.Entities.Comments;
using Domain.Entities.Images;
using Domain.Entities.Items.ItemTrading;
using Domain.Entities.Messages;
using Domain.Entities.ModerationDisappReason;
using Domain.Entities.PaymentMethods;
using Domain.Entities.Reviews;
using Microsoft.EntityFrameworkCore;

namespace Domain.Interfaces.DbContexts
{
    public interface ICatalogDbContext : IDbContext
    {
        public DbSet<ItemTrading> ItemsTrading { get; set; }

        public DbSet<CatalogCategory> CatalogCategories { get; set; }

        public DbSet<CatalogBoolPropertyName> CatalogBoolPropertyNames { get; set; }

        public DbSet<CatalogFloatPropertyName> CatalogFloatPropertyNames { get; set; }

        public DbSet<CatalogIntPropertyName> CatalogIntPropertyNames { get; set; }

        public DbSet<CatalogStringPropertyName> CatalogStringPropertyNames { get; set; }

        public DbSet<CatalogFloatProperty> CatalogItemFloatProperties { get; set; }

        public DbSet<CatalogStringProperty> CatalogItemStringProperties { get; set; }

        public DbSet<CatalogBoolProperty> CatalogItemBoolProperties { get; set; }

        public DbSet<CatalogIntProperty> CatalogItemIntProperties { get; set; }

        public DbSet<ItemTradingImage> ItemsTradingsImages { get; set; }

        public DbSet<ItemTradingSlotImage> ItemsTradingsSlotsImages { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<AttachmentImage<Comment>> CommentsImages { get; set; }

        public DbSet<AttachmentImage<Review>> ReviewImages { get; set; }

        public DbSet<ItemTradingBid> Bids { get; set; }

        public DbSet<Basket> Baskets { get; set; }

        public DbSet<BasketItem> BasketItems { get; set; }

        public DbSet<ItemTradingStatusHistory> ItemTradingStatusHistories { get; set; }

        public DbSet<ItemTradingAuctionSlot> ItemTradingAuctionSlots { get; set; }

        public DbSet<ItemTradingAuctionSlotStatusHistory> ItemTradingAuctionSlotStatusHistories { get; set; }

        public DbSet<ItemTradingModerationDisappReason> ItemTradingModerationDisappReasons { get; set; }

        public DbSet<BannerImage> BannerImages { get; set; }

        public DbSet<ItemTradingQuestion> ItemTradingQuestions { get; set; }

        public DbSet<ItemTradingAnswer> ItemTradingAnswers { get; set; }

        #region userInfo
        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        public DbSet<ItemAddress> Addresses { get; set; }

        public DbSet<UserLogo> UsersLogos { get; set; }

        public DbSet<ShopLogo> ShopsLogos { get; set; }
        #endregion
    }
}
