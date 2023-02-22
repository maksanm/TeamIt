using Models.Teams.Commands;
using System.Net.Http.Json;
using System.Net;
using Domain.Entities.Teams;
using Domain.Enums;

namespace Application.IntegrationTests.Teams.Commands
{
    public class KickTeamMemberCommandTests : TestsBase
    {
        private string _memberToKickId = null;
        private long _teamId;

        [OneTimeSetUp]
        public async Task OneTimeSetup_AddTeamMemberCommandTests()
        {
            var team = CreateTeam();
            var newTeamMember = (await RegisterUser("NewMember")).Item1;

            var context = GetDbContext();
            context.Team.Add(team);
            context.SaveChanges();

            _memberToKickId = newTeamMember.Id;
            _teamId = team.Id;
        }

        [SetUp]
        public void Setup()
        {
            var context = GetDbContext();
            var memberProfile = new TeamProfile()
            {
                UserId = _memberToKickId,
                TeamId = _teamId,
                RoleId = (long)BasicRoleEnum.GUEST
            };
            context.Team.First().Profiles.Add(memberProfile);
            context.SaveChanges();
        }

        [Test]
        public async Task ShouldAddTeamMember()
        {
            var response = await _client.DeleteAsync($"/teams/{_teamId}/members/{_memberToKickId}");

            var context = GetDbContext();
            var teamMembersCount = context.Team.First().Profiles.Count();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(teamMembersCount, Is.EqualTo(1));
        }

        [Test]
        public async Task ShouldReturnBadRequestForWrongUserId()
        {
            var response = await _client.DeleteAsync($"/teams/{_teamId}/members/0");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnBadRequestForWrongTeamId()
        {
            var response = await _client.DeleteAsync($"/teams/0/members/{_memberToKickId}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var response = await _unauthorizedClient.DeleteAsync($"/teams/{_teamId}/members/{_memberToKickId}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}