using Models.Teams.Commands;
using System.Net.Http.Json;
using System.Net;
using Models.Projects.Commands;

namespace Application.IntegrationTests.Projects.Commands
{
    public class CreateProjectCommandTests : TestsBase
    {
        private long _teamId;

        [OneTimeSetUp]
        public void OneTimeSetUp_CreateProjectCommandTests()
        {
            var context = GetDbContext();
            var team = CreateTeam();
            context.Team.Add(team);
            context.SaveChanges();
            _teamId = team.Id;
        }

        [SetUp]
        public void Setup()
        {
            var context = GetDbContext();
            context.ProjectProfile.RemoveRange(context.ProjectProfile);
            context.Project.RemoveRange(context.Project);
            context.SaveChanges();
        }

        [Test]
        public async Task ShouldCreateProject()
        {
            var createProjectCommandDict = new Dictionary<string, string>
            {
                { "Name", "Test team" },
                { "CreatorTeamId", $"{_teamId}" },
                { "LimitRoleName", "Test role" },
                { "LimitRolePermissionIds[0]", "2" },
                { "LimitRolePermissionIds[1]", "3" },
                { "LimitRolePermissionIds[2]", "4" },
                { "UseOwnHierarchy", "true" }
            };
            var createProjectCommand = new HttpRequestMessage(HttpMethod.Post, "/projects")
            {
                Content = new FormUrlEncodedContent(createProjectCommandDict)
            };

            var response = await _client.SendAsync(createProjectCommand);

            var context = GetDbContext();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(context.Project.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForEmptyName()
        {
            var createProjectCommandDict = new Dictionary<string, string>
            {
                { "Name", "" },
                { "CreatorTeamId", $"{_teamId}" },
                { "LimitRoleName", "Test role" },
                { "LimitRolePermissionIds[0]", "2" },
                { "LimitRolePermissionIds[1]", "3" },
                { "LimitRolePermissionIds[2]", "4" },
                { "UseOwnHierarchy", "true" }
            };
            var createProjectCommand = new HttpRequestMessage(HttpMethod.Post, "/projects")
            {
                Content = new FormUrlEncodedContent(createProjectCommandDict)
            };

            var response = await _client.SendAsync(createProjectCommand);

            var context = GetDbContext();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(context.Project.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongCreatorId()
        {
            var createProjectCommandDict = new Dictionary<string, string>
            {
                { "Name", "Test team" },
                { "CreatorTeamId", "0" },
                { "LimitRoleName", "Test role" },
                { "LimitRolePermissionIds[0]", "2" },
                { "LimitRolePermissionIds[1]", "3" },
                { "LimitRolePermissionIds[2]", "4" },
                { "UseOwnHierarchy", "true" }
            };
            var createProjectCommand = new HttpRequestMessage(HttpMethod.Post, "/projects")
            {
                Content = new FormUrlEncodedContent(createProjectCommandDict)
            };

            var response = await _client.SendAsync(createProjectCommand);

            var context = GetDbContext();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(context.Project.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForEmptyLimitRoleName()
        {
            var createProjectCommandDict = new Dictionary<string, string>
            {
                { "Name", "Test team" },
                { "CreatorTeamId", "0" },
                { "LimitRoleName", "" },
                { "LimitRolePermissionIds[0]", "2" },
                { "LimitRolePermissionIds[1]", "3" },
                { "LimitRolePermissionIds[2]", "4" },
                { "UseOwnHierarchy", "true" }
            };
            var createProjectCommand = new HttpRequestMessage(HttpMethod.Post, "/projects")
            {
                Content = new FormUrlEncodedContent(createProjectCommandDict)
            };

            var response = await _client.SendAsync(createProjectCommand);

            var context = GetDbContext();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(context.Project.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongDeadLineDate()
        {
            var createProjectCommandDict = new Dictionary<string, string>
            {
                { "Name", "Test team" },
                { "DeadLine", $"DateTime.Now.AddDays(-1)" },
                { "CreatorTeamId", "0" },
                { "LimitRoleName", "Test role" },
                { "LimitRolePermissionIds[0]", "2" },
                { "LimitRolePermissionIds[1]", "3" },
                { "LimitRolePermissionIds[2]", "4" },
                { "UseOwnHierarchy", "true" }
            };
            var createProjectCommand = new HttpRequestMessage(HttpMethod.Post, "/projects")
            {
                Content = new FormUrlEncodedContent(createProjectCommandDict)
            };

            var response = await _client.SendAsync(createProjectCommand);

            var context = GetDbContext();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(context.Project.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var createProjectCommandDict = new Dictionary<string, string>
            {
                { "Name", "Test team" },
                { "CreatorTeamId", $"{_teamId}" },
                { "LimitRoleName", "Test role" },
                { "LimitRolePermissionIds[0]", "2" },
                { "LimitRolePermissionIds[1]", "3" },
                { "LimitRolePermissionIds[2]", "4" },
                { "UseOwnHierarchy", "true" }
            };
            var createProjectCommand = new HttpRequestMessage(HttpMethod.Post, "/projects")
            {
                Content = new FormUrlEncodedContent(createProjectCommandDict)
            };

            var response = await _unauthorizedClient.SendAsync(createProjectCommand);

            var context = GetDbContext();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            Assert.That(context.Project.Count(), Is.EqualTo(0));
        }
    }
}