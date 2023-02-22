using Models.Projects.Commands;
using System.Net.Http.Json;
using System.Net;

namespace Application.IntegrationTests.Projects.Commands
{
    public class AddTeamToProjectCommandTests : TestsBase
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
        public void SetUp()
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
        }

        [Test]
        public async Task ShouldAddTeamToProject()
        {
            var addTeamToProjectCommand = new AddTeamToProjectCommand()
            {
                ProjectId = _projectId,
                TeamId = _otherTeamId
            };

            var response = await _client.PostAsJsonAsync($"/projects/{_projectId}/team", addTeamToProjectCommand);

            var context = GetDbContext();
            var projectDb = context.Project.First();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(projectDb.Profiles.Count, Is.EqualTo(4));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongProjectId()
        {
            var addTeamToProjectCommand = new AddTeamToProjectCommand()
            {
                ProjectId = 0,
                TeamId = _otherTeamId
            };

            var response = await _client.PostAsJsonAsync("/projects/0/team", addTeamToProjectCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongTeamId()
        {
            var addTeamToProjectCommand = new AddTeamToProjectCommand()
            {
                ProjectId = _projectId,
                TeamId = 0
            };

            var response = await _client.PostAsJsonAsync($"/projects/{_projectId}/team", addTeamToProjectCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnBadRequestIfTryToAddCreatorTeam()
        {
            var addTeamToProjectCommand = new AddTeamToProjectCommand()
            {
                ProjectId = _projectId,
                TeamId = _creatorTeamId
            };

            var response = await _client.PostAsJsonAsync($"/projects/{_projectId}/team", addTeamToProjectCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnBadRequest_IfAlreadyAdded()
        {
            var addTeamToProjectCommand = new AddTeamToProjectCommand()
            {
                ProjectId = _projectId,
                TeamId = _otherTeamId
            };

            await _client.PostAsJsonAsync($"/projects/{_projectId}/team", addTeamToProjectCommand);
            var response = await _client.PostAsJsonAsync($"/projects/{_projectId}/team", addTeamToProjectCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var addTeamToProjectCommand = new AddTeamToProjectCommand()
            {
                ProjectId = _projectId,
                TeamId = _otherTeamId
            };

            var response = await _unauthorizedClient.PostAsJsonAsync($"/projects/{_projectId}/team", addTeamToProjectCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
