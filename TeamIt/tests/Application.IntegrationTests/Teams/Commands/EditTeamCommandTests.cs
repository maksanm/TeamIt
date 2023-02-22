using Models.Teams.Commands;
using System.Net.Http.Json;
using System.Net;

namespace Application.IntegrationTests.Teams.Commands
{
    public class EditTeamCommandTests : TestsBase
    {
        private long _teamId;

        [SetUp]
        public void Setup()
        {
            var context = GetDbContext();
            context.Team.RemoveRange(context.Team);
            var team = CreateTeam();
            context.Team.Add(team);
            context.SaveChanges();
            _teamId = team.Id;
        }

        [Test]
        public async Task ShouldEditTeam()
        {
            var editTeamCommandDict = new Dictionary<string, string>
            {
                { "TeamId", $"{_teamId}" },
                { "Name", "Changed name" },
            };
            var editTeamCommand = new HttpRequestMessage(HttpMethod.Put, $"/teams/{_teamId}")
            {
                Content = new FormUrlEncodedContent(editTeamCommandDict)
            };

            var response = await _client.SendAsync(editTeamCommand);

            var context = GetDbContext();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(context.Team.First().Name, Is.EqualTo("Changed name"));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongId()
        {
            var editTeamCommandDict = new Dictionary<string, string>
            {
                { "TeamId", "0" },
                { "Name", "Changed name" },
            };
            var editTeamCommand = new HttpRequestMessage(HttpMethod.Put, "/teams/0")
            {
                Content = new FormUrlEncodedContent(editTeamCommandDict)
            };

            var response = await _client.SendAsync(editTeamCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForEmptyName()
        {
            var editTeamCommandDict = new Dictionary<string, string>
            {
                { "TeamId", $"{_teamId}" },
                { "Name", "" },
            };
            var editTeamCommand = new HttpRequestMessage(HttpMethod.Put, $"/teams/{_teamId}")
            {
                Content = new FormUrlEncodedContent(editTeamCommandDict)
            };

            var response = await _client.SendAsync(editTeamCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var editTeamCommandDict = new Dictionary<string, string>
            {
                { "TeamId", $"{_teamId}" },
                { "Name", "Changed name" },
            };
            var editTeamCommand = new HttpRequestMessage(HttpMethod.Put, $"/teams/{_teamId}")
            {
                Content = new FormUrlEncodedContent(editTeamCommandDict)
            };

            var response = await _unauthorizedClient.SendAsync(editTeamCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}