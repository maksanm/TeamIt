using Domain.Entities.Teams;
using Infrastructure.Persistance;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Net;
using Domain.Entities;
using Models.Roles.Commands;

namespace Application.IntegrationTests.Roles.Commands
{
    public class EditTeamRoleCommandTests : TestsBase
    {
        private long _teamId;

        [OneTimeSetUp]
        public void OneTimeSetUp_EditTeamRoleCommandTests()
        {
            var context = GetDbContext();
            var team = CreateTeam();
            _teamId = team.Id;
            context.Team.Add(team);
            context.SaveChanges();
        }

        [SetUp]
        public void Setup()
        {
            var context = GetDbContext();

            var roleToDelete = CreateRole(context);
            var contextRole = context.Role.Find(roleToDelete.Id);
            if (contextRole is null)
            {
                context.Role.Add(roleToDelete);
                context.Team.First().Roles.Add(roleToDelete);
                context.SaveChanges();
            }
        }

        [Test]
        public async Task ShouldEditTeamRole()
        {
            var editTeamCommand = new EditTeamRoleCommand()
            {
                TeamId = 1,
                RoleId = 3,
                Name = "Changed role name",
                PermissionIds = new List<int>() { 4, 5, 7, 9 }
            };

            var response = await _client.PutAsJsonAsync($"/teams/{_teamId}/roles", editTeamCommand);

            var context = GetDbContext();
            var roleName = context.Role.Find(editTeamCommand.RoleId).Name;
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(roleName, Is.EqualTo("Changed role name"));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongTeamId()
        {
            var editTeamCommand = new EditTeamRoleCommand()
            {
                TeamId = 0,
                RoleId = 3,
                Name = "Changed role name",
                PermissionIds = new List<int>() { 4, 5, 7, 9 }
            };

            var response = await _client.PutAsJsonAsync("/teams/0/roles", editTeamCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongRoleId()
        {
            var editTeamCommand = new EditTeamRoleCommand()
            {
                TeamId = 1,
                RoleId = 0,
                Name = "Changed role name",
                PermissionIds = new List<int>() { 4, 5, 7, 9 }
            };

            var response = await _client.PutAsJsonAsync($"/teams/{_teamId}/roles", editTeamCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForEmptyName()
        {
            var editTeamCommand = new EditTeamRoleCommand()
            {
                TeamId = 1,
                RoleId = 3,
                Name = "",
                PermissionIds = new List<int>() { 4, 5, 7, 9 }
            };

            var response = await _client.PutAsJsonAsync($"/teams/{_teamId}/roles", editTeamCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForNullPermissionIdsList()
        {
            var editTeamCommand = new EditTeamRoleCommand()
            {
                TeamId = 1,
                RoleId = 3,
                Name = "",
                PermissionIds = null
            };

            var response = await _client.PutAsJsonAsync($"/teams/{_teamId}/roles", editTeamCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var editTeamCommand = new EditTeamRoleCommand()
            {
                TeamId = 1,
                RoleId = 3,
                Name = "Changed role name",
                PermissionIds = new List<int>() { 4, 5, 7, 9 }
            };

            var response = await _unauthorizedClient.PutAsJsonAsync($"/teams/{_teamId}/roles", editTeamCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        private Role CreateRole(ApplicationDbContext context) =>
            new Role()
            {
                Id = 3,
                Name = "Test role",
                Permissions = context.Permission.Take(3).ToList()
            };
    }
}