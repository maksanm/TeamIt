using Models.Tasks.Commands;
using System.Net.Http.Json;
using System.Net;

namespace Application.IntegrationTests.Tasks.Commands
{
    public class AssignUserToTaskCommandTests : TestsBase
    {
        private long _projectId;
        private long _taskId;

        [OneTimeSetUp]
        public void OneTimeSetUp_AssignUserToTaskCommandTests()
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
            task.AssigneeProfileId = null;
            project.Tasks.Add(task);
            context.SaveChanges();
            _taskId = task.Id;
        }

        [Test]
        public async Task ShouldAssignUserToTask()
        {
            var assignUserToTaskCommand = new AssignUserToTaskCommand()
            {
                ProjectId = _projectId,
                TaskId = _taskId,
                UserId = _currentUserId
            };

            var response = await _client.PutAsJsonAsync($"/projects/{_projectId}/tasks/{_taskId}/assign", assignUserToTaskCommand);

            var context = GetDbContext();
            var taskDb = context.Project.Find(_projectId).Tasks.First();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsNotNull(taskDb.AssigneeProfile);
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongProjectId()
        {
            var assignUserToTaskCommand = new AssignUserToTaskCommand()
            {
                ProjectId = 0,
                TaskId = _taskId,
                UserId = _currentUserId
            };

            var response = await _client.PutAsJsonAsync($"/projects/0/tasks/{_taskId}/assign", assignUserToTaskCommand);

            var context = GetDbContext();
            var taskDb = context.Project.Find(_projectId).Tasks.First();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.IsNull(taskDb.AssigneeProfile);
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongTaskId()
        {
            var assignUserToTaskCommand = new AssignUserToTaskCommand()
            {
                ProjectId = _projectId,
                TaskId = 0,
                UserId = _currentUserId
            };

            var response = await _client.PutAsJsonAsync($"/projects/{_projectId}/tasks/0/assign", assignUserToTaskCommand);

            var context = GetDbContext();
            var taskDb = context.Project.Find(_projectId).Tasks.First();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.IsNull(taskDb.AssigneeProfile);
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongUserId()
        {
            var assignUserToTaskCommand = new AssignUserToTaskCommand()
            {
                ProjectId = _projectId,
                TaskId = _taskId,
                UserId = "blablabla"
            };

            var response = await _client.PutAsJsonAsync($"/projects/{_projectId}/tasks/0/assign", assignUserToTaskCommand);

            var context = GetDbContext();
            var taskDb = context.Project.Find(_projectId).Tasks.First();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.IsNull(taskDb.AssigneeProfile);
        }
        
        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var assignUserToTaskCommand = new AssignUserToTaskCommand()
            {
                ProjectId = _projectId,
                TaskId = _taskId,
                UserId = _currentUserId
            };

            var response = await _unauthorizedClient.PutAsJsonAsync($"/projects/{_projectId}/tasks/{_taskId}/assign", assignUserToTaskCommand);

            var context = GetDbContext();
            var taskDb = context.Project.Find(_projectId).Tasks.First();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            Assert.IsNull(taskDb.AssigneeProfile);
        }
    }
}
