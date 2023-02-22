using Blazorise;
using Models.Chats.Commands;
using Models.Chats.Dto;
using Models.Chats.Queries;
using Models.Messages.Commands;
using Models.Messages.Dto;
using Models.Messages.Queries;
using Models.Permissions.Dto;

namespace WebUI.Services
{
	public interface IChatService
	{
		Task<ChatDto> GetChatById(GetChatByIdQuery model);
		Task<ChatInfoDto[]> GetChats(GetCurrentUserChatInfosQuery model);
		Task<UsersToChatWithDto> GetUsersToAddToChat(GetUsersToAddToChatQuery model);
		Task<UsersToChatWithDto> GetUsersToChatWith(GetUsersToChatWithQuery model);
		Task AddUser(AddUserToChatCommand model);
		Task CreateChat(CreateChatCommand model, IFileEntry? image = null);
		Task DeleteChat(DeleteChatCommand model);
		Task EditChat(EditChatCommand model, IFileEntry? image = null);
		Task LeaveChat(LeaveChatCommand model);
		Task<MessageDto[]> GetMessages(GetChatMessagesQuery model);
		Task SendMessage(SendMessageCommand model, IFileEntry? image = null);
		Task<PermissionDto[]> GetCurrentPermissions(GetCurrentUserChatPermissionsQuery model);
	}
	public class ChatService : IChatService
	{
		private IHttpService _httpService;

		public ChatService(IHttpService httpService)
		{
			_httpService = httpService;
		}

		public async Task AddUser(AddUserToChatCommand model)
		{
			await _httpService.Post($"/chats/{model.ChatId}/users", model);
		}

		public async Task CreateChat(CreateChatCommand model, IFileEntry? image = null)
		{
			await _httpService.PostForm($"/chats", model, image);
		}

		public async Task DeleteChat(DeleteChatCommand model)
		{
			await _httpService.Delete($"/chats/{model.ChatId}");
		}

		public async Task EditChat(EditChatCommand model, IFileEntry? image = null)
		{
			await _httpService.PutForm($"/chats/{model.ChatId}", model, image);
		}

		public async Task<ChatDto> GetChatById(GetChatByIdQuery model)
		{
			return await _httpService.Get<ChatDto>($"/chats/{model.ChatId}");
		}

		public async Task<ChatInfoDto[]> GetChats(GetCurrentUserChatInfosQuery model)
		{
			return await _httpService.Get<ChatInfoDto[]>($"/chats");
		}

		public async Task<PermissionDto[]> GetCurrentPermissions(GetCurrentUserChatPermissionsQuery model)
		{
			return await _httpService.Get<PermissionDto[]>($"/chats/{model.ChatId}/permissions");
		}

		public async Task<MessageDto[]> GetMessages(GetChatMessagesQuery model)
		{
			return await _httpService.Get<MessageDto[]>($"/chats/{model.ChatId}/messages/{model.Limit}");
		}

		public async Task<UsersToChatWithDto> GetUsersToAddToChat(GetUsersToAddToChatQuery model)
		{
			return await _httpService.Get<UsersToChatWithDto>($"/chats/{model.ChatId}/users");
		}

		public async Task<UsersToChatWithDto> GetUsersToChatWith(GetUsersToChatWithQuery model)
		{
			return await _httpService.Get<UsersToChatWithDto>($"/chats/users");
		}

		public async Task LeaveChat(LeaveChatCommand model)
		{
			await _httpService.Put($"/chats/{model.ChatId}/leave", model);
		}

		public async Task SendMessage(SendMessageCommand model, IFileEntry? image = null)
		{
			await _httpService.PostForm($"/chats/{model.ChatId}/messages", model, image);
		}
	}
}
