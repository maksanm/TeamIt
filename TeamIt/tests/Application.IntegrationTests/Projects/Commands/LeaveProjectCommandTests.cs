using Domain.Entities.Teams;
using Domain.Enums;
using Models.Projects.Commands;
using Models.Teams.Commands;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Application.IntegrationTests.Projects.Commands
{
    public class LeaveProjectCommandTests : TestsBase
    {
        private long _projectId;
        private long _creatorTeamId;
        private long _leavingTeamId;

        private string _otherUserToken;

        [OneTimeSetUp]
        public async Task OneTimeSetUp_LeaveProjectCommandTests()
        {
            var context = GetDbContext();

            (var otherMember, _otherUserToken) = await RegisterUser("OtherTeamCreator");
            var leavingTeam = CreateTeam(otherMembersCount: 3);
            var creatorTeam = CreateTeam(creatorIdIfNotCurrentUser: otherMember.Id, name: "Other team");

            leavingTeam.Profiles.Add(new TeamProfile()
            {
                TeamId = _leavingTeamId,
                UserId = otherMember.Id,
                RoleId = (long)BasicRoleEnum.TEAM_CREATOR
            });

            context.Team.Add(leavingTeam);
            context.Team.Add(creatorTeam);
            context.SaveChanges();

            _leavingTeamId = leavingTeam.Id;
            _creatorTeamId = creatorTeam.Id;
        }

        [SetUp]
        public async Task SetUp()
        {
            var context = GetDbContext();
            if (context.Project.Any())
            {
                context.Project.RemoveRange(context.Project);
                context.ProjectProfile.RemoveRange(context.ProjectProfile);
            }
            var creatorTeam = context.Team.Find(_creatorTeamId);
            var project = CreateProject(creatorTeam);
            context.Project.Add(project);
            context.SaveChanges();

            _projectId = project.Id;
            await AddTeamToOtherUserProject();
        } 

        [Test]
        public async Task ShouldLeaveProjectWithAllTeamMembers()
        {
            var leaveProjectCommand = new LeaveProjectCommand()
            {
                ProjectId = _projectId,
                TeamId = _leavingTeamId
            };

            var response = await _client.PutAsJsonAsync($"/projects/{_projectId}/leave", leaveProjectCommand);

            var context = GetDbContext();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(context.Project.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongProjectId()
        {
            var leaveProjectCommand = new LeaveProjectCommand()
            {
                ProjectId = 0,
                TeamId = _leavingTeamId
            };

            var response = await _client.PutAsJsonAsync("/projects/0/leave", leaveProjectCommand);
        }
        
        [Test]
        public async Task ShouldReturnBadRequest_ForWrongTeamId()
        {
            var leaveProjectCommand = new LeaveProjectCommand()
            {
                ProjectId = _projectId,
                TeamId = 0
            };

            var response = await _client.PutAsJsonAsync($"/projects/{_projectId}/leave", leaveProjectCommand);
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var leaveProjectCommand = new LeaveProjectCommand()
            {
                ProjectId = _projectId,
                TeamId = _leavingTeamId
            };

            var response = await _unauthorizedClient.PutAsJsonAsync($"/projects/{_projectId}/leave", leaveProjectCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        private async Task AddTeamToOtherUserProject()
        {
            var oldToken = _client.DefaultRequestHeaders.Authorization.Parameter;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _otherUserToken);
            var addTeamToProjectCommand = new AddTeamToProjectCommand()
            {
                ProjectId = _projectId,
                TeamId = _leavingTeamId
            };
            await _client.PostAsJsonAsync($"/projects/{_projectId}/team", addTeamToProjectCommand);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", oldToken);
        }
    }
}
