using Models.Projects.Dto;
using Models.Teams.Dto;
using System.Net;
using System.Net.Http.Json;

namespace Application.IntegrationTests.Projects.Queries
{
    public class GetProjectMembersQueryTests : TestsBase
    {
        private long _projectId;

        [OneTimeSetUp]
        public void OneTimeSetUp_DeleteProjectCommandTests()
        {
            var context = GetDbContext();
            var team = CreateTeam(otherMembersCount: 3);
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
            var project = CreateProject(team);
            context.Project.Add(project);
            context.SaveChanges();

            _projectId = project.Id;
        }

        [Test]
        public async Task ShouldGetProjectTeamMembers()
        {
            var response = await _client.GetAsync($"/projects/{_projectId}/members");
            var projectMembersDtos = await response.Content.ReadFromJsonAsync<ProjectMembersDto>();

            var context = GetDbContext();
            var membersCount = projectMembersDtos
                .Members
                .SelectMany(pmt => pmt.Members)
                .Count();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(projectMembersDtos.Members.Count, Is.EqualTo(1));
            Assert.That(membersCount, Is.EqualTo(4));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongProjectId()
        {
            var response = await _client.GetAsync("/projects/0/members");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var response = await _unauthorizedClient.GetAsync($"/projects/{_projectId}/members");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
