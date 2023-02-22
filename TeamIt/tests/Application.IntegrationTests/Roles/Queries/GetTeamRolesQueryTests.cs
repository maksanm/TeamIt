using Domain.Entities.Teams;
using Models.Roles.Dto;
using Domain.Entities;
using System.Net.Http.Json;
using System.Net;

namespace Application.IntegrationTests.Roles.Queries
{
    // This also tests seeding applied by migrations in the Role table
    public class GetTeamRolesQueryTests : TestsBase
    {
        private long _teamId;

        [OneTimeSetUp]
        public void OneTimeSetUp_GetTeamRolesQueryTests()
        {
            var context = GetDbContext();
            var team = CreateTeam();
            team.Roles.Add(new Role()
            {
                Id = 3,
                Name = "Test role",
            });
            context.Team.Add(team);
            context.SaveChanges();
            _teamId = team.Id;
        }

        [Test]
        public async Task ShouldGetTeamRoles()
        {
            var response = await _client.GetAsync($"/teams/{_teamId}/roles");
            var roleDtos = await response.Content.ReadFromJsonAsync<List<RoleDto>>();

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(roleDtos.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongTeamId()
        {
            var response = await _client.GetAsync("/teams/0/roles");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var response = await _unauthorizedClient.GetAsync($"/teams/{_teamId}/roles");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}