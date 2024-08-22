using Domain.BackendResponses;
using Domain.CoreEnums;
using Domain.Entities.Addresses;
using Domain.Entities.Catalog;
using Domain.Entities.Catalog.CatalogProperty;
using Domain.Entities.Comments;
using Domain.Entities.Images;
using Domain.Entities.Messages;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Domain.CoreEnums.Enums;

namespace Domain.Entities.Items.ItemTrading
{
    public class ItemTrading : ItemBase, IItemTrading, IItemTradingCatalogItem
    {
        /// <summary>
        /// Quantity that is blocked by buying process
        /// </summary>
        public int BlockedQuantity { get; private set; } = 0;

        /// <summary>
        /// Quantity that is on selling
        /// </summary>
        public int FreeQuantity { get; private set; } = 0;

        public List<ItemTradingStatusHistory> ItemTradingStatusHistories { get; private set; } = new();

        public CatalogCategory CatalogCategory { get; private set; }

        public List<CatalogStringProperty> StringProperties { get; set; } = new();

        public List<CatalogFloatProperty> FloatProperties { get; set; } = new();

        public List<CatalogIntProperty> IntProperties { get; set; } = new();

        public List<CatalogBoolProperty> BoolProperties { get; set; } = new();

        public List<ItemAddress> ShipmentAddresses { get; set; } = new();

        public List<ItemTradingImage> Images { get; set; } = new();

        public List<Comment> Comments { get; private set; } = new();

        public List<Enums.PaymentMethod> PaymentMethods { get; set; } = new();

        public SellingTypes SellingType { get; private set; } = SellingTypes.Standard;

        public float MinPrice { get; set; }

        public float MaxPrice { get; set; }

        public DateTime AuctionEndingTime { get; set; }

        public ItemTradingStatus Status { get; private set; } = ItemTradingStatus.Moderation;

        public int DaysForShipment { get; private set; } = 1;

        private List<ItemTradingQuestion> _questions = [];
        [BackingField(nameof(_questions))]
        public IEnumerable<ItemTradingQuestion> Questions { get { return _questions; }}

        private ItemTrading() { }

        public ItemTrading(CatalogCategory catalogCategory,
            Guid userId,
            string title,
            string mainDescription,
            SellingTypes sellingType) : base(userId, title, mainDescription)
        {
            CatalogCategory = catalogCategory;

            SellingType = sellingType;

            CatalogCategory.AddItem(this);
        }

        /// <summary>
        /// Change item status and makes notes in item status history changing
        /// </summary>
        /// <param name="newStatus"></param>
        public void ChangeItemStatus(ItemTradingStatus newStatus)
        {
            if (newStatus == Status) return;

            Status = newStatus;
            StatusChangingLastTime = DateTime.UtcNow;
            ItemTradingStatusHistories.Add(new(this, newStatus));
        }

        /// <summary>
        /// Increase quantity
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns>New value if success, overwise before value</returns>
        public MethodResult<int> AddQuantity(int quantity)
        {
            if (quantity < 0 && quantity > 1000000) return new MethodResult<int>(-1, ["Количество товара вне диапазона"], MethodResults.Conflict);
            if (FreeQuantity + quantity > 1000000) return new MethodResult<int>(-1, ["Количество товара вне диапазона"], MethodResults.Conflict);
            FreeQuantity += quantity;
            return new MethodResult<int>(FreeQuantity, [], MethodResults.Ok);
        }

        /// <summary>
        /// Decrease quantity
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns>New value if seccess, overwise before value</returns>
        public MethodResult<int> RemoveQuantity(int quantity)
        {
            if (quantity < 0) return new MethodResult<int>(-1, ["Количество товара вне диапазона"], MethodResults.Conflict);
            if (FreeQuantity - quantity < 0) return new MethodResult<int>(-1, ["Количество товара вне диапазона"], MethodResults.Conflict);
            FreeQuantity -= quantity;
            return new MethodResult<int>(FreeQuantity, [], MethodResults.Ok);
        }

        public MethodResult SetSimpleItemInfo(string title,
            string description,
            float price,
            List<CatalogStringProperty> stringProperties,
            List<CatalogIntProperty> intProperties,
            List<CatalogFloatProperty> floatProperties,
            List<CatalogBoolProperty> boolProperties,
            List<ItemAddress> shipmentAddresses,
            List<Enums.PaymentMethod> paymentMethods,
            List<ItemTradingImage> images,
            int quantity = 1,
            int daysForShipment = 1)
        {
            var res = SetGeneralInfo(title, description, stringProperties, intProperties, floatProperties, boolProperties, shipmentAddresses, paymentMethods, images, daysForShipment);

            if (res.Result == MethodResults.Conflict) return res;

            if (price <= 0) return new MethodResult(["Цена меньше нуля"], MethodResults.Conflict);
            MinPrice = MaxPrice = price;

            if (quantity <= 0)
                FreeQuantity = 1;
            else
                FreeQuantity = quantity;

            return new MethodResult([], MethodResults.Ok);
        }

        public MethodResult SetAuctionItemInfo(string title,
            string description,
            float minPrice,
            float maxPrice,
            List<CatalogStringProperty> stringProperties,
            List<CatalogIntProperty> intProperties,
            List<CatalogFloatProperty> floatProperties,
            List<CatalogBoolProperty> boolProperties,
            List<ItemAddress> shipmentAddresses,
            List<Enums.PaymentMethod> paymentMethods,
            List<ItemTradingImage> images,
            DateTime auctionEndingTime,
            int daysForShipment = 1)
        {
            var res = SetGeneralInfo(title, description, stringProperties, intProperties, floatProperties, boolProperties, shipmentAddresses, paymentMethods, images, daysForShipment);

            if (res.Result == MethodResults.Conflict) return res;

            if (minPrice <= 0 || maxPrice <= 0 || maxPrice < minPrice) return new MethodResult(["Цена меньше нуля"], MethodResults.Conflict);

            MinPrice = minPrice;

            MaxPrice = maxPrice;

            if ((auctionEndingTime - DateTime.UtcNow).TotalHours < 12) return new MethodResult(["Длительность аукциона должна быть более 12 часов"], MethodResults.Conflict);

            AuctionEndingTime = auctionEndingTime;

            return new MethodResult([], MethodResults.Ok);
        }

        private MethodResult SetGeneralInfo(string title,
            string description,
            List<CatalogStringProperty> stringProperties,
            List<CatalogIntProperty> intProperties,
            List<CatalogFloatProperty> floatProperties,
            List<CatalogBoolProperty> boolProperties,
            List<ItemAddress> shipmentAddresses,
            List<Enums.PaymentMethod> paymentMethods,
            List<ItemTradingImage> images,
            int daysForShipment)
        {
            if (title == null || title == "") return new MethodResult(["Заголовок не должен быть пустым"], MethodResults.Conflict);
            Title = title;

            if (description is not null) Description = description;


            if (stringProperties != null)
            {
                StringProperties.Clear();
                StringProperties.AddRange(stringProperties);
            }

            if (intProperties != null)
            {
                IntProperties.Clear();
                IntProperties.AddRange(intProperties);
            }

            if (floatProperties != null)
            {
                FloatProperties.Clear();
                FloatProperties.AddRange(floatProperties);
            }

            if (boolProperties != null)
            {
                BoolProperties.Clear();
                BoolProperties.AddRange(boolProperties);
            }

            if (shipmentAddresses.Count == 0) return new(["Не указан адрес доставки"], MethodResults.Conflict);
            ShipmentAddresses.Clear();
            ShipmentAddresses.AddRange(shipmentAddresses);

            if (paymentMethods.Count == 0) return new(["Не указан способ оплаты"], MethodResults.Conflict);
            PaymentMethods.Clear();
            PaymentMethods.AddRange(paymentMethods);

            Images.Clear();
            Images.AddRange(images);

            if (daysForShipment == 0) return new(["Срок отправки должен быть как минимум 1 день"], MethodResults.Conflict);
            DaysForShipment = daysForShipment;

            return new MethodResult([], MethodResults.Ok);
        }

        public MethodResult SetInfoOnlyForDbTest(string title,
            string description,
            int price,
            List<CatalogStringProperty> stringProperties,
            List<CatalogIntProperty> intProperties,
            List<CatalogFloatProperty> floatProperties,
            List<CatalogBoolProperty> boolProperties,
            List<ItemTradingImage> images,
            int quantity = 1,
            int auctionSlots = 1)
        {

            if (title == null || title == "") return new MethodResult([], MethodResults.Conflict);
            Title = title;

            if (description == null || description == "") return new MethodResult([], MethodResults.Conflict);
            Description = description;

            if (stringProperties != null)
            {
                StringProperties.Clear();
                StringProperties.AddRange(stringProperties);
            }

            if (intProperties != null)
            {
                IntProperties.Clear();
                IntProperties.AddRange(intProperties);
            }

            if (floatProperties != null)
            {
                FloatProperties.Clear();
                FloatProperties.AddRange(floatProperties);
            }

            if (boolProperties != null)
            {
                BoolProperties.Clear();
                BoolProperties.AddRange(boolProperties);
            }

            Images.Clear();
            Images.AddRange(images);


            if (price <= 0) return new MethodResult([], MethodResults.Conflict);
            MinPrice = MaxPrice = price;

            if (quantity <= 0)
                FreeQuantity = 1;
            else
                FreeQuantity = quantity;

            return new MethodResult([], MethodResults.Ok);
        }

        public void AddNewQuestion(ItemTradingQuestion itemTradingQuestion)
        {
            _questions.Add(itemTradingQuestion);
        }
    }
}