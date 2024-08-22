namespace Domain.Common.Dto
{
    public class QuestionAnswerItemGetResponse
    {
        public DateTime QuestionDateTime { get; }

        public string QuestionText { get; }

        public string AnswerText { get; }

        public QuestionAnswerItemGetResponse(DateTime questionDateTime, string questionText, string answerText)
        {
            QuestionDateTime = questionDateTime;
            QuestionText = questionText;
            AnswerText = answerText;
        }
    }
}
