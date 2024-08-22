using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.CatalogDb
{
    /// <inheritdoc />
    public partial class catalog_v_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "auction");

            migrationBuilder.CreateTable(
                name: "BannerImages",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BannerType = table.Column<int>(type: "integer", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BigImagePath = table.Column<string>(type: "text", nullable: false),
                    SmallImagePath = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannerImages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Baskets",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Baskets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatalogCategories",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SystemName = table.Column<string>(type: "text", nullable: false),
                    ParentCatalogCategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsVisible = table.Column<bool>(type: "boolean", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogCategories_CatalogCategories_ParentCatalogCategoryId",
                        column: x => x.ParentCatalogCategoryId,
                        principalSchema: "auction",
                        principalTable: "CatalogCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentType = table.Column<int>(type: "integer", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    WriterId = table.Column<Guid>(type: "uuid", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopsLogos",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BigImagePath = table.Column<string>(type: "text", nullable: false),
                    SmallImagePath = table.Column<string>(type: "text", nullable: false),
                    OwnerEntity = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopsLogos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersLogos",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BigImagePath = table.Column<string>(type: "text", nullable: false),
                    SmallImagePath = table.Column<string>(type: "text", nullable: false),
                    OwnerEntity = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersLogos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BasketItems",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BasketId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasketItems_Baskets_BasketId",
                        column: x => x.BasketId,
                        principalSchema: "auction",
                        principalTable: "Baskets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CatalogBoolPropertyNames",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CatalogCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SystemName = table.Column<string>(type: "text", nullable: false),
                    IsVisible = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogBoolPropertyNames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogBoolPropertyNames_CatalogCategories_CatalogCategoryId",
                        column: x => x.CatalogCategoryId,
                        principalSchema: "auction",
                        principalTable: "CatalogCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CatalogFloatPropertyNames",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CatalogCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SystemName = table.Column<string>(type: "text", nullable: false),
                    IsVisible = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogFloatPropertyNames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogFloatPropertyNames_CatalogCategories_CatalogCategory~",
                        column: x => x.CatalogCategoryId,
                        principalSchema: "auction",
                        principalTable: "CatalogCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CatalogIntPropertyNames",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CatalogCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SystemName = table.Column<string>(type: "text", nullable: false),
                    IsVisible = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogIntPropertyNames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogIntPropertyNames_CatalogCategories_CatalogCategoryId",
                        column: x => x.CatalogCategoryId,
                        principalSchema: "auction",
                        principalTable: "CatalogCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CatalogStringPropertyNames",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CatalogCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SystemName = table.Column<string>(type: "text", nullable: false),
                    IsVisible = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogStringPropertyNames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogStringPropertyNames_CatalogCategories_CatalogCategor~",
                        column: x => x.CatalogCategoryId,
                        principalSchema: "auction",
                        principalTable: "CatalogCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemsTrading",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BlockedQuantity = table.Column<int>(type: "integer", nullable: false),
                    FreeQuantity = table.Column<int>(type: "integer", nullable: false),
                    CatalogCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentMethods = table.Column<int[]>(type: "integer[]", nullable: false),
                    SellingType = table.Column<int>(type: "integer", nullable: false),
                    MinPrice = table.Column<float>(type: "real", nullable: false),
                    MaxPrice = table.Column<float>(type: "real", nullable: false),
                    AuctionEndingTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DaysForShipment = table.Column<int>(type: "integer", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    StatusChangingLastTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsPromotedByPriority = table.Column<bool>(type: "boolean", nullable: false),
                    PromotionByPriorityStartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DesignPromotionType = table.Column<int>(type: "integer", nullable: false),
                    PromotionByDesignStartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemsTrading", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemsTrading_CatalogCategories_CatalogCategoryId",
                        column: x => x.CatalogCategoryId,
                        principalSchema: "auction",
                        principalTable: "CatalogCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemTradingAuctionSlots",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CatalogCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartPrice = table.Column<float>(type: "real", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    MinimumBid = table.Column<float>(type: "real", nullable: false),
                    BlitzPrice = table.Column<float>(type: "real", nullable: false),
                    AuctionEndingTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PendingOrderFormingStartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AuctionSlotNum = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTradingAuctionSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemTradingAuctionSlots_CatalogCategories_CatalogCategoryId",
                        column: x => x.CatalogCategoryId,
                        principalSchema: "auction",
                        principalTable: "CatalogCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReviewImages",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BigImagePath = table.Column<string>(type: "text", nullable: false),
                    SmallImagePath = table.Column<string>(type: "text", nullable: false),
                    OwnerEntityId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewImages_Reviews_OwnerEntityId",
                        column: x => x.OwnerEntityId,
                        principalSchema: "auction",
                        principalTable: "Reviews",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AddressTitle = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: false),
                    District = table.Column<string>(type: "text", nullable: false),
                    Street = table.Column<string>(type: "text", nullable: false),
                    Building = table.Column<string>(type: "text", nullable: false),
                    Floor = table.Column<string>(type: "text", nullable: false),
                    Flat = table.Column<string>(type: "text", nullable: false),
                    PostIndex = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_ItemsTrading_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "auction",
                        principalTable: "ItemsTrading",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WriterId = table.Column<Guid>(type: "uuid", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    CommentedItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_ItemsTrading_CommentedItemId",
                        column: x => x.CommentedItemId,
                        principalSchema: "auction",
                        principalTable: "ItemsTrading",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemsTradingsImages",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BigImagePath = table.Column<string>(type: "text", nullable: false),
                    SmallImagePath = table.Column<string>(type: "text", nullable: false),
                    OwnerEntityId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemsTradingsImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemsTradingsImages_ItemsTrading_OwnerEntityId",
                        column: x => x.OwnerEntityId,
                        principalSchema: "auction",
                        principalTable: "ItemsTrading",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemTradingModerationDisappReasons",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    ModeratorId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTradingModerationDisappReasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemTradingModerationDisappReasons_ItemsTrading_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "auction",
                        principalTable: "ItemsTrading",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemTradingQuestions",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MessageOwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    WriterId = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTradingQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemTradingQuestions_ItemsTrading_MessageOwnerId",
                        column: x => x.MessageOwnerId,
                        principalSchema: "auction",
                        principalTable: "ItemsTrading",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemTradingStatusHistories",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTradingStatusHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemTradingStatusHistories_ItemsTrading_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "auction",
                        principalTable: "ItemsTrading",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bids",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BidOwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    BidAmount = table.Column<float>(type: "real", nullable: false),
                    BidSlotId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bids_ItemTradingAuctionSlots_BidSlotId",
                        column: x => x.BidSlotId,
                        principalSchema: "auction",
                        principalTable: "ItemTradingAuctionSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CatalogItemBoolProperties",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SystemValue = table.Column<string>(type: "text", nullable: false),
                    ItemTradingAuctionSlotId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PropertyValue = table.Column<string>(type: "text", nullable: false),
                    PropertyNameId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsVisible = table.Column<bool>(type: "boolean", nullable: false),
                    IsInUsing = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogItemBoolProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogItemBoolProperties_CatalogBoolPropertyNames_Property~",
                        column: x => x.PropertyNameId,
                        principalSchema: "auction",
                        principalTable: "CatalogBoolPropertyNames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatalogItemBoolProperties_ItemTradingAuctionSlots_ItemTradi~",
                        column: x => x.ItemTradingAuctionSlotId,
                        principalSchema: "auction",
                        principalTable: "ItemTradingAuctionSlots",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CatalogItemFloatProperties",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemTradingAuctionSlotId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PropertyValue = table.Column<float>(type: "real", nullable: false),
                    PropertyNameId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsVisible = table.Column<bool>(type: "boolean", nullable: false),
                    IsInUsing = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogItemFloatProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogItemFloatProperties_CatalogFloatPropertyNames_Proper~",
                        column: x => x.PropertyNameId,
                        principalSchema: "auction",
                        principalTable: "CatalogFloatPropertyNames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatalogItemFloatProperties_ItemTradingAuctionSlots_ItemTrad~",
                        column: x => x.ItemTradingAuctionSlotId,
                        principalSchema: "auction",
                        principalTable: "ItemTradingAuctionSlots",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CatalogItemFloatProperties_ItemsTrading_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "auction",
                        principalTable: "ItemsTrading",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CatalogItemIntProperties",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemTradingAuctionSlotId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PropertyValue = table.Column<int>(type: "integer", nullable: false),
                    PropertyNameId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsVisible = table.Column<bool>(type: "boolean", nullable: false),
                    IsInUsing = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogItemIntProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogItemIntProperties_CatalogIntPropertyNames_PropertyNa~",
                        column: x => x.PropertyNameId,
                        principalSchema: "auction",
                        principalTable: "CatalogIntPropertyNames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatalogItemIntProperties_ItemTradingAuctionSlots_ItemTradin~",
                        column: x => x.ItemTradingAuctionSlotId,
                        principalSchema: "auction",
                        principalTable: "ItemTradingAuctionSlots",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CatalogItemIntProperties_ItemsTrading_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "auction",
                        principalTable: "ItemsTrading",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CatalogItemStringProperties",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SystemValue = table.Column<string>(type: "text", nullable: false),
                    ItemTradingAuctionSlotId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PropertyValue = table.Column<string>(type: "text", nullable: false),
                    PropertyNameId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsVisible = table.Column<bool>(type: "boolean", nullable: false),
                    IsInUsing = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogItemStringProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogItemStringProperties_CatalogStringPropertyNames_Prop~",
                        column: x => x.PropertyNameId,
                        principalSchema: "auction",
                        principalTable: "CatalogStringPropertyNames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatalogItemStringProperties_ItemTradingAuctionSlots_ItemTra~",
                        column: x => x.ItemTradingAuctionSlotId,
                        principalSchema: "auction",
                        principalTable: "ItemTradingAuctionSlots",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ItemsTradingsSlotsImages",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BigImagePath = table.Column<string>(type: "text", nullable: false),
                    SmallImagePath = table.Column<string>(type: "text", nullable: false),
                    OwnerEntityId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemsTradingsSlotsImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemsTradingsSlotsImages_ItemTradingAuctionSlots_OwnerEntit~",
                        column: x => x.OwnerEntityId,
                        principalSchema: "auction",
                        principalTable: "ItemTradingAuctionSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemTradingAuctionSlotStatusHistories",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemTradingAuctionSlotId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTradingAuctionSlotStatusHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemTradingAuctionSlotStatusHistories_ItemTradingAuctionSlo~",
                        column: x => x.ItemTradingAuctionSlotId,
                        principalSchema: "auction",
                        principalTable: "ItemTradingAuctionSlots",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CommentsImages",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BigImagePath = table.Column<string>(type: "text", nullable: false),
                    SmallImagePath = table.Column<string>(type: "text", nullable: false),
                    OwnerEntityId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentsImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentsImages_Comments_OwnerEntityId",
                        column: x => x.OwnerEntityId,
                        principalSchema: "auction",
                        principalTable: "Comments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ItemTradingAnswers",
                schema: "auction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemTradingQuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MessageOwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    WriterId = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTradingAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemTradingAnswers_ItemTradingQuestions_ItemTradingQuestion~",
                        column: x => x.ItemTradingQuestionId,
                        principalSchema: "auction",
                        principalTable: "ItemTradingQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemTradingAnswers_ItemsTrading_MessageOwnerId",
                        column: x => x.MessageOwnerId,
                        principalSchema: "auction",
                        principalTable: "ItemsTrading",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CatalogBoolPropertyItemTrading",
                schema: "auction",
                columns: table => new
                {
                    BoolPropertiesId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogBoolPropertyItemTrading", x => new { x.BoolPropertiesId, x.ItemsId });
                    table.ForeignKey(
                        name: "FK_CatalogBoolPropertyItemTrading_CatalogItemBoolProperties_Bo~",
                        column: x => x.BoolPropertiesId,
                        principalSchema: "auction",
                        principalTable: "CatalogItemBoolProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatalogBoolPropertyItemTrading_ItemsTrading_ItemsId",
                        column: x => x.ItemsId,
                        principalSchema: "auction",
                        principalTable: "ItemsTrading",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CatalogStringPropertyItemTrading",
                schema: "auction",
                columns: table => new
                {
                    ItemsId = table.Column<Guid>(type: "uuid", nullable: false),
                    StringPropertiesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogStringPropertyItemTrading", x => new { x.ItemsId, x.StringPropertiesId });
                    table.ForeignKey(
                        name: "FK_CatalogStringPropertyItemTrading_CatalogItemStringPropertie~",
                        column: x => x.StringPropertiesId,
                        principalSchema: "auction",
                        principalTable: "CatalogItemStringProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatalogStringPropertyItemTrading_ItemsTrading_ItemsId",
                        column: x => x.ItemsId,
                        principalSchema: "auction",
                        principalTable: "ItemsTrading",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_ItemId",
                schema: "auction",
                table: "Addresses",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BasketItems_BasketId",
                schema: "auction",
                table: "BasketItems",
                column: "BasketId");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_BidSlotId",
                schema: "auction",
                table: "Bids",
                column: "BidSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogBoolPropertyItemTrading_ItemsId",
                schema: "auction",
                table: "CatalogBoolPropertyItemTrading",
                column: "ItemsId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogBoolPropertyNames_CatalogCategoryId",
                schema: "auction",
                table: "CatalogBoolPropertyNames",
                column: "CatalogCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogBoolPropertyNames_SystemName",
                schema: "auction",
                table: "CatalogBoolPropertyNames",
                column: "SystemName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CatalogCategories_ParentCatalogCategoryId",
                schema: "auction",
                table: "CatalogCategories",
                column: "ParentCatalogCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogFloatPropertyNames_CatalogCategoryId",
                schema: "auction",
                table: "CatalogFloatPropertyNames",
                column: "CatalogCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogFloatPropertyNames_SystemName",
                schema: "auction",
                table: "CatalogFloatPropertyNames",
                column: "SystemName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CatalogIntPropertyNames_CatalogCategoryId",
                schema: "auction",
                table: "CatalogIntPropertyNames",
                column: "CatalogCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogIntPropertyNames_SystemName",
                schema: "auction",
                table: "CatalogIntPropertyNames",
                column: "SystemName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItemBoolProperties_ItemTradingAuctionSlotId",
                schema: "auction",
                table: "CatalogItemBoolProperties",
                column: "ItemTradingAuctionSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItemBoolProperties_PropertyNameId",
                schema: "auction",
                table: "CatalogItemBoolProperties",
                column: "PropertyNameId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItemBoolProperties_SystemValue",
                schema: "auction",
                table: "CatalogItemBoolProperties",
                column: "SystemValue",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItemFloatProperties_ItemId",
                schema: "auction",
                table: "CatalogItemFloatProperties",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItemFloatProperties_ItemTradingAuctionSlotId",
                schema: "auction",
                table: "CatalogItemFloatProperties",
                column: "ItemTradingAuctionSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItemFloatProperties_PropertyNameId",
                schema: "auction",
                table: "CatalogItemFloatProperties",
                column: "PropertyNameId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItemFloatProperties_PropertyValue",
                schema: "auction",
                table: "CatalogItemFloatProperties",
                column: "PropertyValue");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItemIntProperties_ItemId",
                schema: "auction",
                table: "CatalogItemIntProperties",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItemIntProperties_ItemTradingAuctionSlotId",
                schema: "auction",
                table: "CatalogItemIntProperties",
                column: "ItemTradingAuctionSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItemIntProperties_PropertyNameId",
                schema: "auction",
                table: "CatalogItemIntProperties",
                column: "PropertyNameId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItemIntProperties_PropertyValue",
                schema: "auction",
                table: "CatalogItemIntProperties",
                column: "PropertyValue");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItemStringProperties_ItemTradingAuctionSlotId",
                schema: "auction",
                table: "CatalogItemStringProperties",
                column: "ItemTradingAuctionSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItemStringProperties_PropertyNameId",
                schema: "auction",
                table: "CatalogItemStringProperties",
                column: "PropertyNameId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItemStringProperties_SystemValue",
                schema: "auction",
                table: "CatalogItemStringProperties",
                column: "SystemValue",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CatalogStringPropertyItemTrading_StringPropertiesId",
                schema: "auction",
                table: "CatalogStringPropertyItemTrading",
                column: "StringPropertiesId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogStringPropertyNames_CatalogCategoryId",
                schema: "auction",
                table: "CatalogStringPropertyNames",
                column: "CatalogCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogStringPropertyNames_SystemName",
                schema: "auction",
                table: "CatalogStringPropertyNames",
                column: "SystemName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CommentedItemId",
                schema: "auction",
                table: "Comments",
                column: "CommentedItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentsImages_OwnerEntityId",
                schema: "auction",
                table: "CommentsImages",
                column: "OwnerEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsTrading_CatalogCategoryId",
                schema: "auction",
                table: "ItemsTrading",
                column: "CatalogCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsTrading_MaxPrice",
                schema: "auction",
                table: "ItemsTrading",
                column: "MaxPrice");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsTrading_MinPrice",
                schema: "auction",
                table: "ItemsTrading",
                column: "MinPrice");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsTrading_Status",
                schema: "auction",
                table: "ItemsTrading",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsTradingsImages_OwnerEntityId",
                schema: "auction",
                table: "ItemsTradingsImages",
                column: "OwnerEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsTradingsSlotsImages_OwnerEntityId",
                schema: "auction",
                table: "ItemsTradingsSlotsImages",
                column: "OwnerEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTradingAnswers_ItemTradingQuestionId",
                schema: "auction",
                table: "ItemTradingAnswers",
                column: "ItemTradingQuestionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemTradingAnswers_MessageOwnerId",
                schema: "auction",
                table: "ItemTradingAnswers",
                column: "MessageOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTradingAuctionSlots_CatalogCategoryId",
                schema: "auction",
                table: "ItemTradingAuctionSlots",
                column: "CatalogCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTradingAuctionSlotStatusHistories_ItemTradingAuctionSlo~",
                schema: "auction",
                table: "ItemTradingAuctionSlotStatusHistories",
                column: "ItemTradingAuctionSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTradingModerationDisappReasons_ItemId",
                schema: "auction",
                table: "ItemTradingModerationDisappReasons",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTradingQuestions_MessageOwnerId",
                schema: "auction",
                table: "ItemTradingQuestions",
                column: "MessageOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTradingStatusHistories_ItemId",
                schema: "auction",
                table: "ItemTradingStatusHistories",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewImages_OwnerEntityId",
                schema: "auction",
                table: "ReviewImages",
                column: "OwnerEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "BannerImages",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "BasketItems",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "Bids",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "CatalogBoolPropertyItemTrading",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "CatalogItemFloatProperties",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "CatalogItemIntProperties",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "CatalogStringPropertyItemTrading",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "CommentsImages",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "ItemsTradingsImages",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "ItemsTradingsSlotsImages",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "ItemTradingAnswers",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "ItemTradingAuctionSlotStatusHistories",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "ItemTradingModerationDisappReasons",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "ItemTradingStatusHistories",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "PaymentMethods",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "ReviewImages",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "ShopsLogos",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "UsersLogos",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "Baskets",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "CatalogItemBoolProperties",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "CatalogFloatPropertyNames",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "CatalogIntPropertyNames",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "CatalogItemStringProperties",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "Comments",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "ItemTradingQuestions",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "Reviews",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "CatalogBoolPropertyNames",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "CatalogStringPropertyNames",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "ItemTradingAuctionSlots",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "ItemsTrading",
                schema: "auction");

            migrationBuilder.DropTable(
                name: "CatalogCategories",
                schema: "auction");
        }
    }
}
