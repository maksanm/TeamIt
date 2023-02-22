using Domain.Entities.Chats;
using Microsoft.AspNetCore.SignalR;

namespace Application.Hubs
{
    public interface IChatHubClient
    {
        Task ReceiveMessage(long chatId, string? senderId, string? text, string? attachedImageUrl);
    }

    public class ChatHub : Hub<IChatHubClient>
    {
        public async Task JoinChatGroup(long chatId) => await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        public async Task LeaveChatGroup(long chatId) => await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
    }
}
