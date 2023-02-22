using Models.Projects.Commands;
using System.Net.Http.Json;
using System.Net;

namespace Application.IntegrationTests.Projects.Commands
{
    public class SetLimitRoleCommandTests : TestsBase
    {
        private long _projectId;

        [OneTimeSetUp]
        public void OneTimeSetUp_EditProjectCommandTests()
        {
            var context = GetDbContext();
            var team = CreateTeam();
            context.Team.Add(team);
            context.SaveChanges();
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
            var team = context.Team.First();
            var project = CreateProject(team);
            context.Project.Add(project);
            context.SaveChanges();

            _projectId = project.Id;
        }

        [Test]
        public async Task ShouldSetLimitRole()
        {
            var setLimitRoleCommand = new SetLimitRoleCommand()
            {
                ProjectId = _projectId,
                Name = "New name",
                PermissionIds = new List<int>() { 1, 2, 3, 4, 5, 6 },
                UseOwnHierarchy = false
            };

            var response = await _client.PutAsJsonAsync($"/projects/{_projectId}/limitrole", setLimitRoleCommand);

            var context = GetDbContext();
            var projectDb = context.Project.First();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(projectDb.LimitRole.Name, Is.EqualTo("New name"));
            Assert.That(projectDb.LimitRole.Permissions.Count(), Is.EqualTo(6));
            Assert.That(projectDb.UseOwnHierarchy, Is.EqualTo(false));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongProjectId()
        {
            var setLimitRoleCommand = new SetLimitRoleCommand()
            {
                ProjectId = 0,
                Name = "New name",
                PermissionIds = new List<int>() { 1, 2, 3, 4, 5, 6 },
                UseOwnHierarchy = false
            };

            var response = await _client.PutAsJsonAsync("/projects/0/limitrole", setLimitRoleCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForEmptyName()
        {
            var setLimitRoleCommand = new SetLimitRoleCommand()
            {
                ProjectId = _projectId,
                Name = "",
                PermissionIds = new List<int>() { 1, 2, 3, 4, 5, 6 },
                UseOwnHierarchy = false
            };

            var response = await _client.PutAsJsonAsync($"/projects/{_projectId}/limitrole", setLimitRoleCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }


        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var setLimitRoleCommand = new SetLimitRoleCommand()
            {
                ProjectId = _projectId,
                Name = "New name",
                PermissionIds = new List<int>() { 1, 2, 3, 4, 5, 6 },
                UseOwnHierarchy = false
            };

            var response = await _unauthorizedClient.PutAsJsonAsync($"/projects/{_projectId}/limitrole", setLimitRoleCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
