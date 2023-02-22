using Models.Projects.Commands;
using System.Net.Http.Json;
using System.Net;
using Models.Tasks.Commands;

namespace Application.IntegrationTests.Tasks.Commands
{
    public class CreateTaskCommandTests : TestsBase
    {
        private long _projectId;

        [OneTimeSetUp]
        public void OneTimeSetUp_CreateTaskCommandTests()
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
            context.SaveChanges();
        }

        [Test]
        public async Task ShouldCreateTask()
        {
            var createTaskCommand = new CreateTaskCommand()
            {
                ProjectId = _projectId,
                Name = "Task",
                Description = "Description",
                DeadLine = DateTime.Now.AddDays(1),
                AssignedUserId = _currentUserId
            };

            var response = await _client.PostAsJsonAsync($"/projects/{_projectId}/tasks", createTaskCommand);

            var context = GetDbContext();
            var projectTasksCount = context.Project.Find(_projectId).Tasks.Count();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(projectTasksCount, Is.EqualTo(1));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongProjectId()
        {
            var createTaskCommand = new CreateTaskCommand()
            {
                ProjectId = 0,
                Name = "Task",
                Description = "Description",
                DeadLine = DateTime.Now.AddDays(1),
                AssignedUserId = _currentUserId
            };

            var response = await _client.PostAsJsonAsync("/projects/0/tasks", createTaskCommand);

            var context = GetDbContext();
            var projectTasksCount = context.Project.Find(_projectId).Tasks.Count();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(projectTasksCount, Is.EqualTo(0));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForEmptyName()
        {
            var createTaskCommand = new CreateTaskCommand()
            {
                ProjectId = _projectId,
                Name = "",
                Description = "Description",
                DeadLine = DateTime.Now.AddDays(1),
                AssignedUserId = _currentUserId
            };

            var response = await _client.PostAsJsonAsync($"/projects/{_projectId}/tasks", createTaskCommand);

            var context = GetDbContext();
            var projectTasksCount = context.Project.Find(_projectId).Tasks.Count();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(projectTasksCount, Is.EqualTo(0));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongDeadLine()
        {
            var createTaskCommand = new CreateTaskCommand()
            {
                ProjectId = _projectId,
                Name = "Name",
                Description = "Description",
                DeadLine = DateTime.Now.AddDays(-1),
                AssignedUserId = _currentUserId
            };

            var response = await _client.PostAsJsonAsync($"/projects/{_projectId}/tasks", createTaskCommand);

            var context = GetDbContext();
            var projectTasksCount = context.Project.Find(_projectId).Tasks.Count();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(projectTasksCount, Is.EqualTo(0));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongAssignedUserId()
        {
            var createTaskCommand = new CreateTaskCommand()
            {
                ProjectId = _projectId,
                Name = "Name",
                Description = "Description",
                DeadLine = DateTime.Now.AddDays(1),
                AssignedUserId = "blablabla"
            };

            var response = await _client.PostAsJsonAsync($"/projects/{_projectId}/tasks", createTaskCommand);

            var context = GetDbContext();
            var projectTasksCount = context.Project.Find(_projectId).Tasks.Count();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(projectTasksCount, Is.EqualTo(0));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var createTaskCommand = new CreateTaskCommand()
            {
                ProjectId = _projectId,
                Name = "Name",
                Description = "Description",
                DeadLine = DateTime.Now.AddDays(1),
                AssignedUserId = _currentUserId
            };

            var response = await _unauthorizedClient.PostAsJsonAsync($"/projects/{_projectId}/tasks", createTaskCommand);

            var context = GetDbContext();
            var projectTasksCount = context.Project.Find(_projectId).Tasks.Count();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            Assert.That(projectTasksCount, Is.EqualTo(0));
        }
    }
}
