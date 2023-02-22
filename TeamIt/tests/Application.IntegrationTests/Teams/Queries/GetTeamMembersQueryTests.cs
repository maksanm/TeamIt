using Domain.Entities.Teams;
using Models.Teams.Dto;
using System.Net.Http.Json;
using System.Net;

namespace Application.IntegrationTests.Teams.Queries
{
    public class GetTeamMembersQueryTests : TestsBase
    {
        private long _teamId;

        [SetUp]
        public void Setup()
        {
            var context = GetDbContext();
            if (!context.Team.Any())
            {
                var team = CreateTeam(otherMembersCount: 5);
                context.Team.Add(team);
                context.SaveChanges();
                _teamId = team.Id;
            }
        }

        [Test]
        public async Task ShouldGetTeamMembers()
        {
            var response = await _client.GetAsync($"/teams/{_teamId}/members");

            var teamMemberDtos = await response.Content.ReadFromJsonAsync<List<TeamMemberDto>>();

            var context = GetDbContext();
            var teamProfilesFromDb = context.Team.First().Profiles;
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(teamMemberDtos!.Count, Is.EqualTo(teamProfilesFromDb.Count));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongId()
        {
            var response = await _client.GetAsync("/teams/0/members");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var response = await _unauthorizedClient.GetAsync($"/teams/{_teamId}/members");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}