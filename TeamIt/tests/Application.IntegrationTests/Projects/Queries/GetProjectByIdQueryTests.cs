using System.Net.Http.Json;
using System.Net;
using Models.Projects.Dto;

namespace Application.IntegrationTests.Projects.Queries
{
    public class GetProjectByIdQueryTests : TestsBase
    {
        private long _projectId;

        [OneTimeSetUp]
        public void OneTimeSetUp_GetProjectByIdQueryTests()
        {
            var context = GetDbContext();
            var team = CreateTeam();
            context.Team.Add(team);
            context.SaveChanges();

            var project = CreateProject(team);
            context.Project.Add(project);
            context.SaveChanges();

            _projectId = project.Id;
        }

        [Test]
        public async Task ShouldGetProjectById()
        {
            var response = await _client.GetAsync($"/projects/{_projectId}");
            var projectDto = await response.Content.ReadFromJsonAsync<ProjectDto>();

            var context = GetDbContext();
            var projectDb = context.Project.First();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(projectDb.Id, Is.EqualTo(projectDto.Id));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongId()
        {
            var response = await _client.GetAsync("/projects/0");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var response = await _unauthorizedClient.GetAsync($"/projects/{_projectId}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
