using Microsoft.JSInterop;
using System.Text.Json;

namespace WebUI.Services
{
	public interface ILocalStorageService
	{
		Task<T?> GetItem<T>(string key);
		Task<string?> GetToken();
		Task SetItem<T>(string key, T value);
		Task SetToken(string value);
		Task RemoveItem(string key);
		Task RemoveToken();
	}
	public class LocalStorageService : ILocalStorageService
	{
		private IJSRuntime _jsRuntime;
		private const string _tokenKey = "TeamIt";

		public LocalStorageService(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
		}

		public async Task<T?> GetItem<T>(string key)
		{
			var itemJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
			return itemJson == null
				? default
				: JsonSerializer.Deserialize<T>(itemJson);
		}

		public async Task SetItem<T>(string key, T value)
		{
			await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value));
		}

		public async Task RemoveItem(string key)
		{
			await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
		}

		public async Task<string?> GetToken()
		{
			return await GetItem<string>(_tokenKey);
		}

		public async Task SetToken(string value)
		{
			await SetItem(_tokenKey, value);
		}

		public async Task RemoveToken()
		{
			await RemoveItem(_tokenKey);
		}
	}
}