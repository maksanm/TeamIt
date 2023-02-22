using Domain.Entities.Teams;
using Models.Teams.Dto;
using System.Net.Http.Json;
using System.Net;

namespace Application.IntegrationTests.Teams.Queries
{
    public class GetCurrentUserTeamsQueryTests : TestsBase
    {
        [SetUp]
        public void Setup()
        {
            var context = GetDbContext();
            if (!context.Team.Any())
            {
                context.Team.AddRange(CreateTeams());
                context.SaveChanges();
            }
        }

        [Test]
        public async Task ShouldReturnCurrentUserTeams()
        {
            var response = await _client.GetAsync("/teams");
            var teamDtos = await response.Content.ReadFromJsonAsync<List<TeamDto>>();

            var context = GetDbContext();
            var teamsFromDb = context.Team.ToList();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(teamDtos.Count, Is.EqualTo(teamsFromDb.Count));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var response = await _unauthorizedClient.GetAsync("/teams");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        private List<Team> CreateTeams() => new List<Team>()
        {
            CreateTeam(_currentUserId, "Name1"),
            CreateTeam(_currentUserId, "Name2"),
            CreateTeam(_currentUserId, "Name3")
        };
    }
}