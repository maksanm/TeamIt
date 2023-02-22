using System.Net;

namespace Application.IntegrationTests.Teams.Commands
{
    public class CreateTeamCommandTests : TestsBase
    {
        [SetUp]
        public void Setup()
        {
            var context = GetDbContext();
            context.Team.RemoveRange(context.Team);
            context.SaveChanges();
        }

        [Test]
        public async Task ShouldCreateTeam()
        {
            var createTeamCommandDict = new Dictionary<string, string>
            {
                { "Name", "Test team" },
            };
            var createTeamCommand = new HttpRequestMessage(HttpMethod.Post, $"/teams")
            {
                Content = new FormUrlEncodedContent(createTeamCommandDict)
            };

            var response = await _client.SendAsync(createTeamCommand);

            var context = GetDbContext();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(context.Team.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForEmptyName()
        {
            var createTeamCommandDict = new Dictionary<string, string>
            {
                { "Name", "" },
            };
            var createTeamCommand = new HttpRequestMessage(HttpMethod.Post, $"/teams")
            {
                Content = new FormUrlEncodedContent(createTeamCommandDict)
            };

            var response = await _client.SendAsync(createTeamCommand);

            var context = GetDbContext();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(context.Team.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForDuplicateName()
        {
            var createTeamCommandDict = new Dictionary<string, string>
            {
                { "Name", "Test team" },
            };
            var createTeamCommand = new HttpRequestMessage(HttpMethod.Post, $"/teams")
            {
                Content = new FormUrlEncodedContent(createTeamCommandDict)
            };

            await _client.SendAsync(createTeamCommand);

            createTeamCommand = new HttpRequestMessage(HttpMethod.Post, $"/teams")
            {
                Content = new FormUrlEncodedContent(createTeamCommandDict)
            };
            var response = await _client.SendAsync(createTeamCommand);

            var context = GetDbContext();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(context.Team.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var createTeamCommandDict = new Dictionary<string, string>
            {
                { "Name", "Test team" },
            };
            var createTeamCommand = new HttpRequestMessage(HttpMethod.Post, $"/teams")
            {
                Content = new FormUrlEncodedContent(createTeamCommandDict)
            };

            var response = await _unauthorizedClient.SendAsync(createTeamCommand);

            var context = GetDbContext();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            Assert.That(context.Team.Count(), Is.EqualTo(0));
        }
    }
}