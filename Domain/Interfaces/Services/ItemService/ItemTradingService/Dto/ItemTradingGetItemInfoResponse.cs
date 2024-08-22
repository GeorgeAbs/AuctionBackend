using Domain.Common.Dto;
using Domain.Interfaces.Services.ItemService.ItemTradingService.Dto.AuctionItem;
using Domain.Interfaces.Services.ItemService.ItemTradingService.Dto.PropertiesForGetItemRequest;
using static Domain.CoreEnums.Enums;

namespace Domain.Interfaces.Services.ItemService.ItemTradingService.Dto
{
    public class ItemTradingGetItemInfoResponse
    {
        public Guid Id { get; }

        public string Title { get; }

        public string Description { get; }

        public IEnumerable<string> ImagesURLs { get; }

        public SellingTypes SellingType { get; }

        public IEnumerable<ItemTradingAuctionSlotInfoGetItemResponse> Slots { get; private set; } = [];

        public DateTime AuctionEndingTime { get; private set; }

        public int Quantity { get; }

        public float Price { get; }

        public string CatalogName { get;}

        public IEnumerable<IntItemPropertyGetItemResponse> IntProperties { get; }

        public IEnumerable<FloatItemPropertyGetItemResponse> FloatProperties { get; }

        public IEnumerable<StringItemPropertyGetItemResponse> StringProperties { get; }

        public IEnumerable<BoolItemPropertyGetItemResponse> BoolProperties { get; }

        public IEnumerable<QuestionAnswerItemGetResponse> QuestionsAnswers { get; private set; }

        public IEnumerable<ReviewResponseItemGetResponse> ReviewsResponses { get; private set; }

        public ItemTradingGetItemInfoResponse(Guid id,
            string title,
            string description,
            IEnumerable<string> imagesURLs,
            SellingTypes sellingType,
            DateTime auctionEndingTime,
            int quantity,
            float price,
            string catalogName,
            IEnumerable<IntItemPropertyGetItemResponse> intProperties,
            IEnumerable<FloatItemPropertyGetItemResponse> floatProperties,
            IEnumerable<StringItemPropertyGetItemResponse> stringProperties,
            IEnumerable<BoolItemPropertyGetItemResponse> boolProperties)
        {
            Id = id;
            Title = title;
            Description = description;
            ImagesURLs = imagesURLs;
            SellingType = sellingType;
            AuctionEndingTime = auctionEndingTime;
            Quantity = quantity;
            Price = price;
            CatalogName = catalogName;
            IntProperties = intProperties;
            FloatProperties = floatProperties;
            StringProperties = stringProperties;
            BoolProperties = boolProperties;
        }

        public void SetAuctionSlots(IEnumerable<ItemTradingAuctionSlotInfoGetItemResponse> slots)
        {
            Slots = slots;
        }

        public void SetQuestionsAnswers(IEnumerable<QuestionAnswerItemGetResponse> questionsAnswers)
        {
            QuestionsAnswers = questionsAnswers;
        }

        public void SetReviewsResponses(IEnumerable<ReviewResponseItemGetResponse> reviewsResponses)
        {
            ReviewsResponses = reviewsResponses;
        }
    }
}
