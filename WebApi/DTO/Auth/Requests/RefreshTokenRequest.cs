namespace WebApi.DTO.Auth.Requests
{
    public class RefreshTokenRequest
    {
        public string AccessToken { get; }

        public string RefreshToken { get; }

        public RefreshTokenRequest(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
