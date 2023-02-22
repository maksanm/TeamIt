using Models.Tasks.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Application.IntegrationTests.Tasks.Queries
{
    public class GetTaskByIdQueryTests : TestsBase
    {
        private long _projectId;
        private long _taskId;

        [OneTimeSetUp]
        public void OneTimeSetUp_GetTaskByIdQueryTests()
        {
            var context = GetDbContext();
            var team = CreateTeam();
            context.Team.Add(team);
            context.SaveChanges();
            var project = CreateProject(team);
            context.Project.Add(project);
            context.SaveChanges();
            var task = CreateTask(project);
            project.Tasks.Add(task);
            context.SaveChanges();
            _projectId = project.Id;
            _taskId = task.Id;
        }

        [Test]
        public async Task ShouldDeleteTask()
        {
            var response = await _client.GetAsync($"/projects/{_projectId}/tasks/{_taskId}");
            var taskDto = await response.Content.ReadFromJsonAsync<TaskDto>();

            var context = GetDbContext();
            var taskDb = context.Project.Find(_projectId).Tasks.First();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(taskDb.Name, Is.EqualTo(taskDto.Name));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongProjectId()
        {
            var response = await _client.GetAsync($"/projects/0/tasks/{_taskId}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongTaskId()
        {
            var response = await _client.GetAsync($"/projects/{_projectId}/tasks/0");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var response = await _unauthorizedClient.GetAsync($"/projects/{_projectId}/tasks/{_taskId}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
