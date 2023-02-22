using Blazorise;
using Models.Roles.Commands;
using Models.Roles.Dto;
using Models.Roles.Queries;
using Models.Teams.Commands;
using Models.Teams.Dto;
using Models.Teams.Queries;
using Models.Users.Commands;

namespace WebUI.Services
{
	public interface ITeamService
	{
		Task<TeamDto[]> GetTeams(GetCurrentUserTeamsQuery model);

		Task<TeamDto> GetTeamById(GetTeamByIdQuery model);

		Task CreateTeam(CreateTeamCommand model, IFileEntry? image = null);

		Task EditTeam(EditTeamCommand model, IFileEntry? image = null);

		Task DeleteTeam(DeleteTeamCommand model);

		Task LeaveTeam(LeaveTeamCommand model);

		Task<TeamMemberDto[]> GetMembers(GetTeamMembersQuery model);

		Task AddMember(SendJoinTeamRequestCommand model);

		Task AssignMemberRole(AssignTeamMemberRoleCommand model);

		Task KickMember(KickTeamMemberCommand model);

		Task<RoleDto[]> GetRoles(GetTeamRolesQuery model);

		Task<RoleDto> GetCurrentRole(GetCurrentUserTeamRoleQuery model);

		Task CreateRole(CreateTeamRoleCommand model);

		Task ChangeRole(EditTeamRoleCommand model);

		Task DeleteRole(DeleteTeamRoleCommand model);

		Task<JoinTeamRequestDto[]> GetJoinRequests(GetCurrentUserJoinTeamRequestsQuery model);

		Task AnswerJoinRequest(AnswerJoinTeamRequestCommand model);

		Task SetTeamCreator(ChangeTeamCreatorCommand model);
	}

	public class TeamService : ITeamService
	{
		private IHttpService _httpService;

		public TeamService(IHttpService httpService)
		{
			_httpService = httpService;
		}

		public async Task AddMember(SendJoinTeamRequestCommand model)
		{
			await _httpService.Post($"/teams/{model.TeamId}/members", model);
		}

		public async Task AssignMemberRole(AssignTeamMemberRoleCommand model)
		{
			await _httpService.Put($"/teams/{model.TeamId}/members", model);
		}

		public async Task CreateRole(CreateTeamRoleCommand model)
		{
			await _httpService.Post($"/teams/{model.TeamId}/roles", model);
		}

		public async Task CreateTeam(CreateTeamCommand model, IFileEntry? image = null)
		{
			await _httpService.PostForm("/teams", model, image);
		}

		public async Task DeleteRole(DeleteTeamRoleCommand model)
		{
			await _httpService.Delete($"/teams/{model.TeamId}/roles/{model.RoleId}");
		}

		public async Task DeleteTeam(DeleteTeamCommand model)
		{
			await _httpService.Delete($"/teams/{model.TeamId}");
		}

		public async Task EditTeam(EditTeamCommand model, IFileEntry? image = null)
		{
			await _httpService.PutForm($"/teams/{model.TeamId}", model, image);
		}

		public async Task<RoleDto> GetCurrentRole(GetCurrentUserTeamRoleQuery model)
		{
			return await _httpService.Get<RoleDto>($"/teams/{model.TeamId}/role");
		}

		public async Task<TeamMemberDto[]> GetMembers(GetTeamMembersQuery model)
		{
			return await _httpService.Get<TeamMemberDto[]>($"/teams/{model.TeamId}/members");
		}

		public async Task<RoleDto[]> GetRoles(GetTeamRolesQuery model)
		{
			return await _httpService.Get<RoleDto[]>($"/teams/{model.TeamId}/roles");
		}

		public async Task<TeamDto[]> GetTeams(GetCurrentUserTeamsQuery model)
		{
			return await _httpService.Get<TeamDto[]>("/teams");
		}

		public async Task<TeamDto> GetTeamById(GetTeamByIdQuery model)
		{
			return await _httpService.Get<TeamDto>($"/teams/{model.TeamId}/get");
		}

		public async Task KickMember(KickTeamMemberCommand model)
		{
			await _httpService.Delete($"/teams/{model.TeamId}/members/{model.UserId}");
		}

		public async Task LeaveTeam(LeaveTeamCommand model)
		{
			await _httpService.Put($"/teams/{model.TeamId}/leave", model);
		}

		public async Task ChangeRole(EditTeamRoleCommand model)
		{
			await _httpService.Put($"/teams/{model.TeamId}/roles", model);
		}

		public async Task<JoinTeamRequestDto[]> GetJoinRequests(GetCurrentUserJoinTeamRequestsQuery model)
		{
			return await _httpService.Get<JoinTeamRequestDto[]>($"/teams/invites");
		}

		public async Task AnswerJoinRequest(AnswerJoinTeamRequestCommand model)
		{
			await _httpService.Post($"/teams/invites/{model.RequestId}/answer", model);
		}

		public async Task SetTeamCreator(ChangeTeamCreatorCommand model)
		{
			await _httpService.Put($"/teams/{model.TeamId}/creator", model);
		}
	}
}