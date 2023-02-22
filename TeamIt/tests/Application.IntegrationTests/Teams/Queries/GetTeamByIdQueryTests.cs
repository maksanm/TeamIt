using Domain.Entities.Teams;
using Models.Teams.Dto;
using System.Net;
using System.Net.Http.Json;

namespace Application.IntegrationTests.Teams.Queries
{
    public class GetTeamByIdQueryTests : TestsBase
    {
        private long _teamId;

        [SetUp]
        public void Setup()
        {
            var context = GetDbContext();
            if (!context.Team.Any())
            {
                var team = CreateTeam();
                context.Team.Add(team);
                context.SaveChanges();
                _teamId = team.Id;
            }
        }

        [Test]
        public async Task ShouldGetTeamById()
        {
            var response = await _client.GetAsync($"/teams/{_teamId}/get");

            var teamDto = await response.Content.ReadFromJsonAsync<TeamDto>();

            var context = GetDbContext();
            var teamFromDb = context.Team.First();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(teamDto.Name, Is.EqualTo(teamFromDb.Name));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongId()
        {
            var response = await _client.GetAsync("/teams/0/get");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var response = await _unauthorizedClient.GetAsync($"/teams/{_teamId}/get");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}