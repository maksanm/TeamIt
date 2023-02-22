using Models.Tasks.Commands;
using System.Net.Http.Json;
using System.Net;

namespace Application.IntegrationTests.Tasks.Commands
{
    public class DeleteTaskCommandTests : TestsBase
    {
        private long _projectId;
        private long _taskId;

        [OneTimeSetUp]
        public void OneTimeSetUp_DeleteTaskCommandTests()
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

        [SetUp]
        public void Setup()
        {
            var context = GetDbContext();
            context.Task.RemoveRange(context.Task);
            var project = context.Project.Find(_projectId);
            var task = CreateTask(project);
            project.Tasks.Add(task);
            context.SaveChanges();
            _taskId = task.Id;
        }

        [Test]
        public async Task ShouldDeleteTask()
        {
            var response = await _client.DeleteAsync($"/projects/{_projectId}/tasks/{_taskId}");

            var context = GetDbContext();
            var projectTasksCount = context.Project.Find(_projectId).Tasks.Count();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(projectTasksCount, Is.EqualTo(0));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongProjectId()
        {
            var response = await _client.DeleteAsync($"/projects/0/tasks/{_taskId}");

            var context = GetDbContext();
            var projectTasksCount = context.Project.Find(_projectId).Tasks.Count();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(projectTasksCount, Is.EqualTo(1));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongTaskId()
        {
            var response = await _client.DeleteAsync($"/projects/{_projectId}/tasks/0");

            var context = GetDbContext();
            var projectTasksCount = context.Project.Find(_projectId).Tasks.Count();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(projectTasksCount, Is.EqualTo(1));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var response = await _unauthorizedClient.DeleteAsync($"/projects/{_projectId}/tasks/{_taskId}");

            var context = GetDbContext();
            var projectTasksCount = context.Project.Find(_projectId).Tasks.Count();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            Assert.That(projectTasksCount, Is.EqualTo(1));
        }
    }
}
