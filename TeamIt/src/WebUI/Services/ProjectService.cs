using Blazorise;
using Models.Permissions.Dto;
using Models.Projects.Commands;
using Models.Projects.Dto;
using Models.Projects.Queries;
using Models.Tasks.Commands;
using Models.Tasks.Dto;
using Models.Tasks.Queries;
using Models.Teams.Dto;

namespace WebUI.Services
{
	public interface IProjectService
	{
		Task<ProjectDto> GetProjectById(GetProjectByIdQuery model);
		Task<ProjectInfoDto[]> GetCurrentUserProjects(GetCurrentUserProjectInfosQuery model);
		Task<ProjectMembersDto> GetProjectMembers(GetProjectMembersQuery model);
		Task AddTeamToProject(AddTeamToProjectCommand model);
		Task CreateProject(CreateProjectCommand model, IFileEntry? image = null);
		Task DeleteProject(DeleteProjectCommand model);
		Task EditProject(EditProjectCommand model, IFileEntry? image = null);
		Task KickTeamFromProject(KickTeamFromProjectCommand model);
		Task LeaveProject(LeaveProjectCommand model);
		Task SetLimitRole(SetLimitRoleCommand model);
		Task AssignUserToTask(AssignUserToTaskCommand model);
		Task CreateTask(CreateTaskCommand model);
		Task DeleteTask(DeleteTaskCommand model);
		Task EditTask(EditTaskCommand model);
		Task<TaskDto[]> GetTasks(GetProjectTasksQuery model);
		Task<TaskDto> GetTaskById(GetTaskByIdQuery model);
		Task<PermissionDto[]> GetCurrentPermissions(GetCurrentUserProjectPermissionsQuery model);
		Task<TeamDto[]> GetTeamsToAddToProject(GetTeamsToAddToProjectQuery model);
	}
	public class ProjectService : IProjectService
	{
		private IHttpService _httpService;

		public ProjectService(IHttpService httpService)
		{
			_httpService = httpService;
		}

		public async Task AddTeamToProject(AddTeamToProjectCommand model)
		{
			await _httpService.Post($"/projects/{model.ProjectId}/team", model);
		}

		public async Task AssignUserToTask(AssignUserToTaskCommand model)
		{
			await _httpService.Put($"/projects/{model.ProjectId}/tasks/{model.TaskId}/assign", model);
		}

		public async Task CreateProject(CreateProjectCommand model, IFileEntry? image = null)
		{
			await _httpService.PostForm($"/projects", model, image);
		}

		public async Task CreateTask(CreateTaskCommand model)
		{
			await _httpService.Post($"/projects/{model.ProjectId}/tasks", model);
		}

		public async Task DeleteProject(DeleteProjectCommand model)
		{
			await _httpService.Delete($"/projects/{model.ProjectId}");
		}

		public async Task DeleteTask(DeleteTaskCommand model)
		{
			await _httpService.Delete($"/projects/{model.ProjectId}/tasks/{model.TaskId}");
		}

		public async Task EditProject(EditProjectCommand model, IFileEntry? image = null)
		{
			await _httpService.PutForm($"/projects/{model.ProjectId}", model, image);
		}

		public async Task EditTask(EditTaskCommand model)
		{
			await _httpService.Put($"/projects/{model.ProjectId}/tasks/{model.TaskId}", model);
		}

		public async Task<PermissionDto[]> GetCurrentPermissions(GetCurrentUserProjectPermissionsQuery model)
		{
			return await _httpService.Get<PermissionDto[]>($"/projects/{model.ProjectId}/permissions");
		}

		public async Task<ProjectInfoDto[]> GetCurrentUserProjects(GetCurrentUserProjectInfosQuery model)
		{
			return await _httpService.Get<ProjectInfoDto[]>($"/projects");
		}

		public async Task<ProjectDto> GetProjectById(GetProjectByIdQuery model)
		{
			return await _httpService.Get<ProjectDto>($"/projects/{model.ProjectId}");
		}

		public async Task<ProjectMembersDto> GetProjectMembers(GetProjectMembersQuery model)
		{
			return await _httpService.Get<ProjectMembersDto>($"/projects/{model.ProjectId}/members");
		}

		public async Task<TaskDto> GetTaskById(GetTaskByIdQuery model)
		{
			return await _httpService.Get<TaskDto>($"/projects/{model.ProjectId}/tasks/{model.TaskId}");
		}

		public async Task<TaskDto[]> GetTasks(GetProjectTasksQuery model)
		{
			return await _httpService.Get<TaskDto[]>($"/projects/{model.ProjectId}/tasks");
		}

		public async Task KickTeamFromProject(KickTeamFromProjectCommand model)
		{
			await _httpService.Delete($"/projects/{model.ProjectId}/team/{model.TeamId}");
		}

		public async Task LeaveProject(LeaveProjectCommand model)
		{
			await _httpService.Put($"/projects/{model.ProjectId}/leave", model);
		}

		public async Task SetLimitRole(SetLimitRoleCommand model)
		{
			await _httpService.Put($"/projects/{model.ProjectId}/limitrole", model);
		}

		public async Task<TeamDto[]> GetTeamsToAddToProject(GetTeamsToAddToProjectQuery model)
		{
			return await _httpService.Get<TeamDto[]>($"/projects/{model.ProjectId}/members/{model.Name}");
		}
	}
}
