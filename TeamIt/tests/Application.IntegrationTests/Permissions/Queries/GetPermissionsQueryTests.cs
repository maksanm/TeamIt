using Models.Teams.Dto;
using System.Net.Http.Json;
using System.Net;
using Domain.Enums;

namespace Application.IntegrationTests.Permissions.Queries
{
    // This also tests seeding applied by migrations in the Permission table
    public class GetPermissionsQueryTests : TestsBase
    {
        [Test]
        public async Task ShouldGetAllSeededPermissions()
        {
            var response = await _client.GetAsync("/permissions");

            var permissionDtos = await response.Content.ReadFromJsonAsync<List<TeamDto>>();

            var domainEnumCount = Enum.GetValues<PermissionEnum>().Length;
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(permissionDtos.Count, Is.EqualTo(domainEnumCount));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var response = await _unauthorizedClient.GetAsync("/permissions");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}