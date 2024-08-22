using System.ComponentModel.DataAnnotations;

namespace WebApi.DTO.Moderation
{
    public class ModerationDisapproveRequest
    {
        [Required]
        public Guid ItemId { get; set; }

        [Required]
        public string Message { get; set; }

        public ModerationDisapproveRequest(Guid itemId, string message)
        {
            ItemId = itemId;
            Message = message;
        }
    }
}
