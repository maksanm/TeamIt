using Domain.Entities.Teams;
using System.Net.Http.Json;
using System.Net;
using Models.Roles.Dto;

namespace Application.IntegrationTests.Roles.Queries
{
    public class GetCurrentUserTeamRoleQueryTests : TestsBase
    {
        private long _teamId;

        [OneTimeSetUp]
        public void OneTimeSetUp_GetCurrentUserTeamsQueryTests()
        {
            var context = GetDbContext();
            var team = CreateTeam();
            context.Team.Add(team);
            context.SaveChanges();
            _teamId = team.Id;
        }

        [Test]
        public async Task ShouldGetCurrentUserTeamRole()
        {
            var response = await _client.GetAsync($"/teams/{_teamId}/role");
            var roleDto = await response.Content.ReadFromJsonAsync<RoleDto>();

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(roleDto.Id, Is.EqualTo(1));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongTeamId()
        {
            var response = await _client.GetAsync("/teams/0/role");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var response = await _unauthorizedClient.GetAsync($"/teams/{_teamId}/role");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}