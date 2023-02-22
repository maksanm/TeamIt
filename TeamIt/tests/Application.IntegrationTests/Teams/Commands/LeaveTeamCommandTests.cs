using Domain.Entities.Teams;
using Models.Teams.Commands;
using System.Net.Http.Json;
using System.Net;

namespace Application.IntegrationTests.Teams.Commands
{
    public class LeaveTeamCommandTests : TestsBase
    {
        private string _teamCreatorUserId;
        private long _teamId;

        [OneTimeSetUp]
        public async Task OneTimeSetUp_LeaveTeamCommandTests()
        {
            _teamCreatorUserId = (await RegisterUser("Creator")).Item1.Id;
            var context = GetDbContext();
            var team = CreateTeam(_teamCreatorUserId);
            context.Team.Add(team);
            context.SaveChanges();
            _teamId = team.Id;
        }

        [SetUp]
        public void Setup()
        {
            var context = GetDbContext();
            var team = context.Team.First();
            if (!team.Profiles.Any(tp => tp.UserId == _currentUserId))
            {
                var currentUserProfile = new TeamProfile()
                {
                    UserId = _currentUserId,
                    TeamId = _teamId,
                    RoleId = 2
                };
                team.Profiles.Add(currentUserProfile);
                var currentUser = context.User.Find(_currentUserId);
                currentUser.TeamProfiles.Add(currentUserProfile);
                context.SaveChanges();
            }
        }

        [Test]
        public async Task ShouldLeaveTeam()
        {
            var editTeamCommand = new LeaveTeamCommand()
            {
                TeamId = _teamId
            };

            var response = await _client.PutAsJsonAsync($"/teams/{_teamId}/leave", editTeamCommand);

            var context = GetDbContext();
            var teamMembersCount = context.Team.First().Profiles.Count();
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(teamMembersCount, Is.EqualTo(1));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongId()
        {
            var editTeamCommand = new LeaveTeamCommand()
            {
                TeamId = 0
            };

            var response = await _client.PutAsJsonAsync("/teams/0/leave", editTeamCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var editTeamCommand = new LeaveTeamCommand()
            {
                TeamId = _teamId
            };

            var response = await _unauthorizedClient.PutAsJsonAsync($"/teams/{_teamId}/leave", editTeamCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}