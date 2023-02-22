using Models.Tasks.Dto;
using System.Net;
using System.Net.Http.Json;

namespace Application.IntegrationTests.Tasks.Queries
{
    public class GetProjectTasksQueryTests : TestsBase
    {
        private long _projectId;

        [OneTimeSetUp]
        public void OneTimeSetUp_GetCurrentUserTaskInfosQueryTests()
        {
            var context = GetDbContext();
            var team = CreateTeam();
            context.Team.Add(team);
            context.SaveChanges();
            var project = CreateProject(team, tasksCount: 5);
            context.Project.Add(project);
            context.SaveChanges();
            _projectId = project.Id;
        }

        [Test]
        public async Task ShouldHetCirrentUserTaskInfos()
        {
            var response = await _client.GetAsync($"/projects/{_projectId}/tasks");
            var taskDtos = await response.Content.ReadFromJsonAsync<List<TaskDto>>();

            var context = GetDbContext();
            var tasksCountDb = context.Project.Find(_projectId).Tasks.Count();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(tasksCountDb, Is.EqualTo(taskDtos.Count));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongProjectId()
        {
            var response = await _client.GetAsync($"/projects/0/tasks/");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var response = await _unauthorizedClient.GetAsync($"/projects/{_projectId}/tasks/");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
