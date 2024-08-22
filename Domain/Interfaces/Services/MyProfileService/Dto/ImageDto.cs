namespace Domain.Interfaces.Services.MyProfileService.Dto
{
    public class ImageDto
    {
        public string BigImageUrl { get; set; }

        public string SmallImageUrl { get; set; }

        public ImageDto(string bigImageUrl, string smallImageUrl)
        {
            BigImageUrl = bigImageUrl;
            SmallImageUrl = smallImageUrl;
        }
    }
}
