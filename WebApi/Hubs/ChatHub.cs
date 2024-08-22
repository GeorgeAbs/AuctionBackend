using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;
using WebApi.Constants;

namespace WebApi.Hubs
{
    [SignalRHub(HubsPaths.CHAT_HUB_PATH)]
    [Authorize(AuthenticationSchemes = "Identity.Application," + JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub : Hub
    {
    }
}
