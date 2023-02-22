using Models.Teams.Commands;
using System.Net.Http.Json;
using System.Net;

namespace Application.IntegrationTests.Teams.Commands
{
    public class SendJoinTeamRequestCommandTests : TestsBase
    {
        private string _newMemberId = null;
        private long _teamId;

        [OneTimeSetUp]
        public async Task OneTimeSetup_AddTeamMemberCommandTests()
        {
            var newTeamMember = (await RegisterUser("NewMember")).Item1;
            _newMemberId = newTeamMember.Id;
        }

        [SetUp]
        public void Setup()
        {
            var team = CreateTeam();
            var context = GetDbContext();
            context.JoinTeamRequest.RemoveRange(context.JoinTeamRequest);
            context.Team.RemoveRange(context.Team);
            _teamId = team.Id;
            context.Team.Add(team);
            context.SaveChanges();
        }

        [Test]
        public async Task ShouldJoinTeamRequest()
        {
            var addTeamMemberCommand = new SendJoinTeamRequestCommand()
            {
                TeamId = 1,
                UserId = _newMemberId
            };

            var response = await _client.PostAsJsonAsync($"/teams/{_teamId}/members", addTeamMemberCommand);

            var context = GetDbContext();
            var teamInvitesCount = context.JoinTeamRequest.Count();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(teamInvitesCount, Is.EqualTo(1));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongUserId()
        {
            var addTeamMemberCommand = new SendJoinTeamRequestCommand()
            {
                TeamId = 1,
                UserId = "blablabla"
            };

            var response = await _client.PostAsJsonAsync($"/teams/{_teamId}/members", addTeamMemberCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongTeamId()
        {
            var addTeamMemberCommand = new SendJoinTeamRequestCommand()
            {
                TeamId = 0,
                UserId = _newMemberId
            };

            var response = await _client.PostAsJsonAsync("/teams/0/members", addTeamMemberCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var addTeamMemberCommand = new SendJoinTeamRequestCommand()
            {
                TeamId = 1,
                UserId = _newMemberId
            };

            var response = await _unauthorizedClient.PostAsJsonAsync($"/teams/{_teamId}/members", addTeamMemberCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}