using Models.Teams.Dto;
using System.Net.Http.Json;
using System.Net;
using Domain.Entities.ProjectManager;
using Domain.Entities.Teams;
using Models.Projects.Dto;

namespace Application.IntegrationTests.Projects.Queries
{
    public class GetCurrentUserProjectInfosQueryTests : TestsBase
    {
        [SetUp]
        public void Setup()
        {
            var context = GetDbContext();
            if (!context.Team.Any())
            {
                var team = CreateTeam();
                context.Team.Add(team);
                context.SaveChanges();
                context.Project.AddRange(CreateProjects(team));
            }
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldReturnCurrentProjectInfos()
        {
            var response = await _client.GetAsync("/projects");
            var projectDtos = await response.Content.ReadFromJsonAsync<List<ProjectInfoDto>>();

            var context = GetDbContext();
            var projectsFromDb = context.Project.ToList();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(projectsFromDb.Count, Is.EqualTo(projectDtos.Count));
        }

        [Test]
        public async System.Threading.Tasks.Task ShouldReturnUnauthorized()
        {
            var response = await _unauthorizedClient.GetAsync("/projects");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        private List<Project> CreateProjects(Team team) => new List<Project>()
        {
            CreateProject(team, "Project1", 1),
            CreateProject(team, "Project2", 2),
            CreateProject(team, "Project3", 3)
        };
    }
}
