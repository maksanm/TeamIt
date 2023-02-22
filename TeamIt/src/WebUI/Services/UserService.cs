using Blazorise;
using Microsoft.AspNetCore.Components;
using Models.Users.Commands;
using Models.Users.Dto;
using Models.Users.Queries;

namespace WebUI.Services
{
	public interface IUserService
	{
		UserDto? User { get; }
		Task Initialize();
		bool IsLoggedIn();
		Task Login(LoginCommand model);
		Task Logout();
		Task Register(RegisterCommand model, IFileEntry? image = null);
		Task<UserDto[]> GetByName(GetUsersWithNameQuery model);
	}

	public class UserService : IUserService
	{
		private IHttpService _httpService;
		private NavigationManager _navigationManager;
		private ILocalStorageService _localStorageService;
		public UserDto? User { get; private set; }

		public UserService(
			IHttpService httpService,
			NavigationManager navigationManager,
			ILocalStorageService localStorageService
		)
		{
			_httpService = httpService;
			_navigationManager = navigationManager;
			_localStorageService = localStorageService;
		}

		public async Task Initialize()
		{
			await GetCurrentUser();
		}

		public bool IsLoggedIn()
		{
			return User is not null;
		}

		public async Task Login(LoginCommand model)
		{
			var Token = await _httpService.Post<string>("/users/login", model);
			await _localStorageService.SetToken(Token);
			await GetCurrentUser();
		}

		public async Task Logout()
		{
			User = null;
			await _localStorageService.RemoveToken();
			_navigationManager.NavigateTo("account/login", true, true);
		}

		public async Task Register(RegisterCommand model, IFileEntry? image = null)
		{
			var Token = await _httpService.PostForm<string>("/users/register", model, image);
			await _localStorageService.SetToken(Token);
			await GetCurrentUser();
		}

		public async Task<UserDto[]> GetByName(GetUsersWithNameQuery model)
		{
			return await _httpService.Get<UserDto[]>($"/users/{model.Name}");
		}

		private async Task GetCurrentUser()
		{
			if (!string.IsNullOrEmpty(await _localStorageService.GetToken()))
			{
				User = await _httpService.Get<UserDto>("/users/current");
			}
		}
	}
}