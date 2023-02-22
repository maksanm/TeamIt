using Models.Enums;
using Models.Tasks.Commands;
using System.Net;
using System.Net.Http.Json;

namespace Application.IntegrationTests.Tasks.Commands
{
    public class EditTaskCommandTests : TestsBase
    {
        private long _projectId;
        private long _taskId;

        [OneTimeSetUp]
        public void OneTimeSetUp_EditTaskCommandTests()
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
            var editTaskCommand = new EditTaskCommand()
            {
                ProjectId = _projectId,
                TaskId = _taskId,
                Name = "New name",
                Description = "New description",
                State = TaskStateEnumDto.TODO,
                DeadLine = DateTime.Now.AddDays(1)
            };

            var response = await _client.PutAsJsonAsync($"/projects/{_projectId}/tasks/{_taskId}", editTaskCommand);

            var context = GetDbContext();
            var taskDb = context.Project.Find(_projectId).Tasks.First();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(taskDb.Name, Is.EqualTo("New name"));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongProjectId()
        {
            var editTaskCommand = new EditTaskCommand()
            {
                ProjectId = 0,
                TaskId = _taskId,
                Name = "New name",
                Description = "New description",
                State = TaskStateEnumDto.TODO,
                DeadLine = DateTime.Now.AddDays(1)
            };

            var response = await _client.PutAsJsonAsync($"/projects/0/tasks/{_taskId}", editTaskCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongTaskId()
        {
            var editTaskCommand = new EditTaskCommand()
            {
                ProjectId = _projectId,
                TaskId = 0,
                Name = "New name",
                Description = "New description",
                State = TaskStateEnumDto.TODO,
                DeadLine = DateTime.Now.AddDays(1)
            };

            var response = await _client.PutAsJsonAsync($"/projects/{_projectId}/tasks/0", editTaskCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongDeadLine()
        {
            var editTaskCommand = new EditTaskCommand()
            {
                ProjectId = _projectId,
                TaskId = _taskId,
                Name = "New name",
                Description = "New description",
                State = TaskStateEnumDto.TODO,
                DeadLine = DateTime.Now.AddDays(-1)
            };

            var response = await _client.PutAsJsonAsync($"/projects/{_projectId}/tasks/{_taskId}", editTaskCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongEmptyName()
        {
            var editTaskCommand = new EditTaskCommand()
            {
                ProjectId = _projectId,
                TaskId = _taskId,
                Name = "",
                Description = "New description",
                State = TaskStateEnumDto.TODO,
                DeadLine = DateTime.Now.AddDays(1)
            };

            var response = await _client.PutAsJsonAsync($"/projects/{_projectId}/tasks/{_taskId}", editTaskCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var editTaskCommand = new EditTaskCommand()
            {
                ProjectId = _projectId,
                TaskId = _taskId,
                Name = "New name",
                Description = "New description",
                State = TaskStateEnumDto.TODO,
                DeadLine = DateTime.Now.AddDays(1)
            };

            var response = await _unauthorizedClient.PutAsJsonAsync($"/projects/{_projectId}/tasks/{_taskId}", editTaskCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
