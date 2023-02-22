using Models.Projects.Commands;
using System.Net.Http.Json;
using System.Net;

namespace Application.IntegrationTests.Projects.Commands
{
    public class KickTeamFromProjectCommandTests : TestsBase
    {
        private long _projectId;
        private long _creatorTeamId;
        private long _otherTeamId;

        [OneTimeSetUp]
        public async Task OneTimeSetUp_AddTeamToProjectCommandTests()
        {
            var context = GetDbContext();
            var team = CreateTeam();
            var otherTeam = CreateTeam(name: "Other team", otherMembersCount: 3);

            context.Team.Add(team);
            context.Team.Add(otherTeam);
            context.SaveChanges();

            _creatorTeamId = team.Id;
            _otherTeamId = otherTeam.Id;
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
            var addTeamToProjectCommand = new AddTeamToProjectCommand()
            {
                ProjectId = _projectId,
                TeamId = _otherTeamId
            };
            await _client.PostAsJsonAsync($"/projects/{_projectId}/team", addTeamToProjectCommand);
        }

        [Test]
        public async Task ShouldKickTeamMembersFromProject()
        {
            var response = await _client.DeleteAsync($"/projects/{_projectId}/team/{_otherTeamId}");

            var context = GetDbContext();
            var projectDb = context.Project.First();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(projectDb.Profiles.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongProjectId()
        {
            var response = await _client.DeleteAsync($"/projects/0/team/{_otherTeamId}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongTeamId()
        {
            var response = await _client.DeleteAsync($"/projects/{_projectId}/team/0");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
        
        [Test]
        public async Task ShouldReturnBadRequest_IfKickCreatorTeam()
        {
            var response = await _client.DeleteAsync($"/projects/{_projectId}/team/{_creatorTeamId}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var response = await _unauthorizedClient.DeleteAsync($"/projects/{_projectId}/team/{_otherTeamId}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
