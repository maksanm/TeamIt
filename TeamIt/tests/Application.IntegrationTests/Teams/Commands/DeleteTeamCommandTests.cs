using Domain.Entities.Teams;
using System.Net;

namespace Application.IntegrationTests.Teams.Commands
{
    public class DeleteTeamCommandTests : TestsBase
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
            context.RemoveRange(context.Team);
        }

        [Test]
        public async Task ShouldDeleteTeam()
        {
            var response = await _client.DeleteAsync($"/teams/{_teamId}");

            var context = GetDbContext();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(context.Team.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongId()
        {
            var response = await _client.DeleteAsync("/teams/0");

            var context = GetDbContext();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(context.Team.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var response = await _unauthorizedClient.DeleteAsync($"/teams/{_teamId}");

            var context = GetDbContext();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            Assert.That(context.Team.Count(), Is.EqualTo(1));
        }
    }
}