using System.Net;

namespace Application.IntegrationTests.Projects.Commands
{
    public class DeleteProjectCommandTests : TestsBase
    {
        private long _projectId;

        [OneTimeSetUp]
        public void OneTimeSetUp_DeleteProjectCommandTests()
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
                return;
            var team = context.Team.First();
            var project = CreateProject(team, tasksCount: 3);
            context.Project.Add(project);
            context.SaveChanges();

            _projectId = project.Id;
        }

        [Test]
        public async Task ShouldDeleteProjectWithTasksAndProfiles()
        {
            var response = await _client.DeleteAsync($"/projects/{_projectId}");

            var context = GetDbContext();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(context.Project.Count(), Is.EqualTo(0));
            Assert.That(context.ProjectProfile.Count(), Is.EqualTo(0));
            Assert.That(context.Task.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongId()
        {
            var response = await _client.DeleteAsync("/projects/0");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var response = await _unauthorizedClient.DeleteAsync($"/projects/{_projectId}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
