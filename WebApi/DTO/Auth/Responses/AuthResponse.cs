namespace WebApi.DTO.Auth.Responses
{
    public class AuthResponse
    {
        public string AccessToken { get;  }

        public string RefreshToken { get;  }

        public AuthResponse(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
