using Models.Projects.Dto;
using System.Net;
using System.Net.Http.Json;

namespace Application.IntegrationTests.Projects.Queries
{
    public class GetCurrentUserProjectPermissionsQueryTests : TestsBase
    {
        private long _projectId;

        [SetUp]
        public void Setup()
        {
            var context = GetDbContext();
            if (!context.Team.Any())
            {
                var team = CreateTeam();
                context.Team.Add(team);
                context.SaveChanges();
                var project = CreateProject(team);
                context.Project.Add(project);
                context.SaveChanges();
                _projectId = project.Id;
            }
        }

        [Test]
        public async Task ShouldReturnCurrentProjectInfos()
        {
            var response = await _client.GetAsync($"/projects/{_projectId}/permissions");
            var permissionDtos = await response.Content.ReadFromJsonAsync<List<ProjectInfoDto>>();

            var context = GetDbContext();
            var permissionsFromDb = context.Permission.ToList();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(permissionsFromDb.Count, Is.EqualTo(permissionDtos.Count));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongIds()
        {
            var response = await _client.GetAsync("/projects/0/permissions");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var response = await _unauthorizedClient.GetAsync($"/projects/{_projectId}/permissions");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
