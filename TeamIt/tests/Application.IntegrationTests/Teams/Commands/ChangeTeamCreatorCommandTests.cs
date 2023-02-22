using Models.Teams.Commands;
using System.Net.Http.Json;
using System.Net;
using Domain.Entities.Teams;
using Domain.Enums;

namespace Application.IntegrationTests.Teams.Commands
{
    public class ChangeTeamCreatorCommandTests : TestsBase
    {
        private string _newCreatorId;
        private long _teamId;

        [OneTimeSetUp]
        public async Task OneTimeSetup_AddTeamMemberCommandTests()
        {
            var newTeamMember = (await RegisterUser("NewMember")).Item1;
            _newCreatorId = newTeamMember.Id;
        }

        [SetUp]
        public void Setup()
        {
            var context = GetDbContext();
            context.Team.RemoveRange(context.Team);
            var team = CreateTeam();
            var memberProfile = new TeamProfile()
            {
                UserId = _newCreatorId,
                TeamId = _teamId,
                RoleId = (long)BasicRoleEnum.TEAM_CREATOR
            };
            team.Profiles.Add(memberProfile);

            var newCreatorUser = context.User.Find(_newCreatorId);
            newCreatorUser.TeamProfiles.Add(memberProfile);

            context.Team.Add(team);
            context.SaveChanges();

            _teamId = team.Id;
        }

        [Test]
        public async Task ShouldChangeTeamCreator()
        {
            var changeTeamCreatorCommand = new ChangeTeamCreatorCommand()
            {
                TeamId = _teamId,
                NewTeamCreatorUserId = _newCreatorId
            };

            var response = await _client.PutAsJsonAsync($"/teams/{_teamId}/creator", changeTeamCreatorCommand);

            var context = GetDbContext();
            var teamCreatorIdDb = context.Team.First().CreatorUserId;
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(teamCreatorIdDb, Is.EqualTo(_newCreatorId));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongTeamId()
        {
            var changeTeamCreatorCommand = new ChangeTeamCreatorCommand()
            {
                TeamId = 0,
                NewTeamCreatorUserId = _newCreatorId
            };

            var response = await _client.PutAsJsonAsync($"/teams/{_teamId}/creator", changeTeamCreatorCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForCreatorId()
        {
            var changeTeamCreatorCommand = new ChangeTeamCreatorCommand()
            {
                TeamId = _teamId,
                NewTeamCreatorUserId = "blablabla"
            };

            var response = await _client.PutAsJsonAsync($"/teams/{_teamId}/creator", changeTeamCreatorCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var changeTeamCreatorCommand = new ChangeTeamCreatorCommand()
            {
                TeamId = _teamId,
                NewTeamCreatorUserId = _newCreatorId
            };

            var response = await _unauthorizedClient.PutAsJsonAsync($"/teams/{_teamId}/creator", changeTeamCreatorCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}