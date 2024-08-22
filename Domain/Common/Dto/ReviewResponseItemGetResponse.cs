namespace Domain.Common.Dto
{
    public class ReviewResponseItemGetResponse
    {
        public DateTime ReviewDateTime { get; }

        public string ItemTitle { get; }

        public string ReviewText { get; }

        public int Mark {  get; }

        public IEnumerable<string> ImagesURLs { get; } = [];

        public string AnswerText { get; }

        public ReviewResponseItemGetResponse(DateTime reviewDateTime, string itemTitle, string reviewText, int mark, IEnumerable<string> imagesURLs, string answerText)
        {
            ReviewDateTime = reviewDateTime;
            ItemTitle = itemTitle;
            ReviewText = reviewText;
            Mark = mark;
            ImagesURLs = imagesURLs;
            AnswerText = answerText;
        }
    }
}
