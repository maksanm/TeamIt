using System.Net;

namespace Application.IntegrationTests.Projects.Commands
{
    public class EditProjectCommandTests : TestsBase
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
                context.Task.RemoveRange(context.Task);
            }
            var team = context.Team.First();
            var project = CreateProject(team, tasksCount: 3);
            context.Project.Add(project);
            context.SaveChanges();

            _projectId = project.Id;
        }

        [Test]
        public async Task ShouldEditProject()
        {
            var deadLine = DateTime.Now.AddDays(1);
            deadLine = new DateTime(deadLine.Year, deadLine.Month, deadLine.Day, deadLine.Hour, deadLine.Minute, deadLine.Second, deadLine.Kind);
            var editProjectCommandDict = new Dictionary<string, string>
            {
                { "ProjectId", $"{_projectId}" },
                { "Name", "New name" },
                { "Description", "New description" },
                { "DeadLine", $"{deadLine}" }
            };
            var editProjectCommand = new HttpRequestMessage(HttpMethod.Put, $"/projects/{_projectId}")
            {
                Content = new FormUrlEncodedContent(editProjectCommandDict)
            };

            var response = await _client.SendAsync(editProjectCommand);

            var context = GetDbContext();
            var projectDb = context.Project.First();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(projectDb.Name, Is.EqualTo("New name"));
            Assert.That(projectDb.Description, Is.EqualTo("New description"));
            Assert.That(projectDb.DeadLine, Is.EqualTo(deadLine));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongProjectId()
        {
            var editProjectCommandDict = new Dictionary<string, string>
            {
                { "ProjectId", "0" },
                { "Name", "New name" },
                { "Description", "New description" }
            };
            var editProjectCommand = new HttpRequestMessage(HttpMethod.Put, "/projects/0")
            {
                Content = new FormUrlEncodedContent(editProjectCommandDict)
            };

            var response = await _client.SendAsync(editProjectCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongDeadLineDate()
        {
            var deadLine = DateTime.Now.AddDays(-1);
            deadLine = new DateTime(deadLine.Year, deadLine.Month, deadLine.Day, deadLine.Hour, deadLine.Minute, deadLine.Second, deadLine.Kind);
            var editProjectCommandDict = new Dictionary<string, string>
            {
                { "ProjectId", $"{_projectId}" },
                { "Name", "New name" },
                { "Description", "New description" },
                { "DeadLine", $"{deadLine}" }
            };
            var editProjectCommand = new HttpRequestMessage(HttpMethod.Put, $"/projects/{_projectId}")
            {
                Content = new FormUrlEncodedContent(editProjectCommandDict)
            };

            var response = await _client.SendAsync(editProjectCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }


        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var editProjectCommandDict = new Dictionary<string, string>
            {
                { "ProjectId", $"{_projectId}" },
                { "Name", "New name" },
                { "Description", "New description" }
            };
            var editProjectCommand = new HttpRequestMessage(HttpMethod.Put, $"/projects/{_projectId}")
            {
                Content = new FormUrlEncodedContent(editProjectCommandDict)
            };

            var response = await _unauthorizedClient.SendAsync(editProjectCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
