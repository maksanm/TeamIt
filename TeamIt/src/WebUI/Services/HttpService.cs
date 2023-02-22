using Blazorise;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using WebUI.Helpers;

namespace WebUI.Services
{
	public interface IHttpService
	{
		string ApiUrl { get; set; }
		Task<T> Get<T>(string uri);
		Task Post(string uri, object value);
		Task PostForm(string uri, object value, IFileEntry? image = null);
		Task<T> Post<T>(string uri, object value);
		Task<T> PostForm<T>(string uri, object value, IFileEntry? image = null);
		Task Put(string uri, object value);
		Task PutForm(string uri, object value, IFileEntry? image = null);
		Task<T> Put<T>(string uri, object value);
		Task<T> PutForm<T>(string uri, object value, IFileEntry? image = null);
		Task Delete(string uri);
		Task<T> Delete<T>(string uri);
	}

	public class HttpService : IHttpService
	{
		private HttpClient _httpClient;
		private NavigationManager _navigationManager;
		private ILocalStorageService _localStorageService;

		public string ApiUrl { get; set; }

		public HttpService(
			HttpClient httpClient,
			NavigationManager navigationManager,
			ILocalStorageService localStorageService
		)
		{
			_httpClient = httpClient;
			_navigationManager = navigationManager;
			_localStorageService = localStorageService;
		}

		public async Task<T> Get<T>(string uri)
		{
			var request = new HttpRequestMessage(HttpMethod.Get, uri);
			return await sendRequest<T>(request);
		}

		public async Task Post(string uri, object value)
		{
			var request = createRequest(HttpMethod.Post, uri, value);
			await sendRequest(request);
		}

		public async Task<T> Post<T>(string uri, object value)
		{
			var request = createRequest(HttpMethod.Post, uri, value);
			return await sendRequest<T>(request);
		}

		public async Task Put(string uri, object value)
		{
			var request = createRequest(HttpMethod.Put, uri, value);
			await sendRequest(request);
		}

		public async Task<T> Put<T>(string uri, object value)
		{
			var request = createRequest(HttpMethod.Put, uri, value);
			return await sendRequest<T>(request);
		}

		public async Task Delete(string uri)
		{
			var request = createRequest(HttpMethod.Delete, uri);
			await sendRequest(request);
		}

		public async Task<T> Delete<T>(string uri)
		{
			var request = createRequest(HttpMethod.Delete, uri);
			return await sendRequest<T>(request);
		}

		private HttpRequestMessage createRequest(HttpMethod method, string uri, object? value = null)
		{
			var request = new HttpRequestMessage(method, uri);
			if (value != null)
				request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
			return request;
		}

		private HttpRequestMessage createRequestForm(HttpMethod method, string uri, object? value = null, IFileEntry? image = null)
		{
			var request = new HttpRequestMessage(method, uri);
			if (value != null)
				request.Content = value.ToFormData(image);
			return request;
		}

		private async Task sendRequest(HttpRequestMessage request)
		{
			await addJwtHeader(request);

			using var response = await _httpClient.SendAsync(request);

			if (response.StatusCode == HttpStatusCode.Unauthorized)
			{
				_navigationManager.NavigateTo("account/logout");
				return;
			}

			await handleErrors(response);
		}

		private async Task<T> sendRequest<T>(HttpRequestMessage request)
		{
			await addJwtHeader(request);

			using var response = await _httpClient.SendAsync(request);

			if (response.StatusCode == HttpStatusCode.Unauthorized)
			{
				_navigationManager.NavigateTo("account/login");
				return default;
			}
			if (response.StatusCode == HttpStatusCode.BadRequest)
			{
				var content = await response.Content.ReadAsStringAsync();
				if (content == "User is unauthorized")
				{
					await _localStorageService.RemoveToken();
					_navigationManager.NavigateTo("account/login");
				}
				else
				{
					throw new Exception();
				}
				return default;
			}

			await handleErrors(response);

			if (typeof(string).IsAssignableFrom(typeof(T)))
			{
				return (T)Convert.ChangeType(await response.Content.ReadAsStringAsync(), typeof(T));
			}

			return await response.Content.ReadFromJsonAsync<T>();
		}

		private async Task addJwtHeader(HttpRequestMessage request)
		{
			var token = await _localStorageService.GetToken();
			if (token != null)
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
		}

		private async Task handleErrors(HttpResponseMessage response)
		{
			if (!response.IsSuccessStatusCode)
			{
				var error = await response.Content.ReadAsStringAsync();
				throw new Exception(error);
			}
		}

		public async Task PostForm(string uri, object value, IFileEntry? image = null)
		{
			var request = createRequestForm(HttpMethod.Post, uri, value, image);
			await sendRequest(request);
		}

		public async Task<T> PostForm<T>(string uri, object value, IFileEntry? image = null)
		{
			var request = createRequestForm(HttpMethod.Post, uri, value, image);
			return await sendRequest<T>(request);
		}

		public async Task PutForm(string uri, object value, IFileEntry? image = null)
		{
			var request = createRequestForm(HttpMethod.Put, uri, value, image);
			await sendRequest(request);
		}

		public async Task<T> PutForm<T>(string uri, object value, IFileEntry? image = null)
		{
			var request = createRequestForm(HttpMethod.Put, uri, value, image);
			return await sendRequest<T>(request);
		}
	}
}