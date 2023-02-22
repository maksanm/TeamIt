using Models.Teams.Commands;
using System.Net.Http.Json;
using System.Net;
using Domain.Entities.Teams;
using Domain.Enums;

namespace Application.IntegrationTests.Teams.Commands
{
    public class AssignTeamMemberRoleCommandTests : TestsBase
    {
        private string _memberId = null;
        private long _teamId;

        [OneTimeSetUp]
        public async Task OneTimeSetup_AssignTeamMemberRoleCommandTests()
        {
            var team = CreateTeam();
            var newTeamMember = (await RegisterUser("NewMember")).Item1;
            var memberProfile = new TeamProfile()
            {
                UserId = newTeamMember.Id,
                TeamId = _teamId,
                RoleId = (long)BasicRoleEnum.GUEST
            };
            team.Profiles.Add(memberProfile);
            newTeamMember.TeamProfiles.Add(memberProfile);

            var context = GetDbContext();
            context.Team.Add(team);
            context.SaveChanges();

            _memberId = newTeamMember.Id;
            _teamId = team.Id;
        }

        [SetUp]
        public void Setup()
        {
            var context = GetDbContext();
            var memberProfile = context.TeamProfile.First(tp => tp.UserId == _memberId);
            memberProfile.RoleId = (long)BasicRoleEnum.GUEST;
            context.SaveChanges();
        }

        [Test]
        public async Task ShouldAssignTeamMemberRole()
        {
            var assignTeamMemberRoleCommand = new AssignTeamMemberRoleCommand()
            {
                UserId = _memberId,
                RoleId = (long)BasicRoleEnum.TEAM_CREATOR,
                TeamId = _teamId
            };

            var response = await _client.PutAsJsonAsync($"/teams/{_teamId}/members", assignTeamMemberRoleCommand);

            var context = GetDbContext();
            var releIdDb = context.TeamProfile.First(tp => tp.UserId == _memberId).RoleId;
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(releIdDb, Is.EqualTo((long)BasicRoleEnum.TEAM_CREATOR));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongUserId()
        {
            var assignTeamMemberRoleCommand = new AssignTeamMemberRoleCommand()
            {
                UserId = "blablabla",
                RoleId = (long)BasicRoleEnum.TEAM_CREATOR,
                TeamId = _teamId
            };

            var response = await _client.PutAsJsonAsync($"/teams/{_teamId}/members", assignTeamMemberRoleCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongTeamId()
        {
            var assignTeamMemberRoleCommand = new AssignTeamMemberRoleCommand()
            {
                UserId = _memberId,
                RoleId = (long)BasicRoleEnum.TEAM_CREATOR,
                TeamId = 0
            };

            var response = await _client.PutAsJsonAsync("/teams/0/members", assignTeamMemberRoleCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongRoleId()
        {
            var assignTeamMemberRoleCommand = new AssignTeamMemberRoleCommand()
            {
                UserId = _memberId,
                RoleId = 0,
                TeamId = _teamId
            };

            var response = await _client.PutAsJsonAsync("/teams/0/members", assignTeamMemberRoleCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var assignTeamMemberRoleCommand = new AssignTeamMemberRoleCommand()
            {
                UserId = _memberId,
                RoleId = (long)BasicRoleEnum.TEAM_CREATOR,
                TeamId = _teamId
            };

            var response = await _unauthorizedClient.PutAsJsonAsync($"/teams/{_teamId}/members", assignTeamMemberRoleCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}