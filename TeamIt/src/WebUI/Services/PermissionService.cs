using Models.Permissions.Dto;
using Models.Permissions.Queries;

namespace WebUI.Services
{
	public interface IPermissionService
	{
		Task<PermissionDto[]> GetPermissions(GetPermissionsQuery model);
	}
	public class PermissionService : IPermissionService
	{
		private IHttpService _httpService;

		public PermissionService(IHttpService httpService)
		{
			_httpService = httpService;
		}

		public async Task<PermissionDto[]> GetPermissions(GetPermissionsQuery model)
		{
			return await _httpService.Get<PermissionDto[]>("/permissions");
		}
	}
}