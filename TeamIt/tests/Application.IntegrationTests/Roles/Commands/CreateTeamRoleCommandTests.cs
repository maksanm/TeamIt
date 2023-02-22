using Models.Teams.Commands;
using System.Net.Http.Json;
using System.Net;
using Domain.Entities.Teams;
using Models.Roles.Commands;
using Domain.Entities;
using Infrastructure.Persistance;
using Microsoft.Extensions.DependencyInjection;
using Application.Common.Interfaces;

namespace Application.IntegrationTests.Roles.Commands
{
    public class CreateTeamRoleCommandTests : TestsBase
    {
        [SetUp]
        public void Setup()
        {
            var context = GetDbContext();
            context.RemoveRange(context.Team);
            context.Team.Add(CreateTeamWithSeededRoles(context));
            context.SaveChanges();
        }

        [Test]
        public async Task ShouldCreateTeamRole()
        {
            var createTeamRoleCommand = new CreateTeamRoleCommand()
            {
                TeamId = 1,
                Name = "Test role",
                PermissionIds = new List<int>() { 1, 2, 4, 6 }
            };

            var response = await _client.PostAsJsonAsync("/teams/1/roles", createTeamRoleCommand);

            var context = GetDbContext();
            var teamRolesCountDb = context.Team.First().Roles.Count;
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(teamRolesCountDb, Is.EqualTo(3));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForEmptyName()
        {
            var createTeamRoleCommand = new CreateTeamRoleCommand()
            {
                TeamId = 1,
                Name = "",
                PermissionIds = new List<int>() { 1, 2, 4, 6 }
            };

            var response = await _client.PostAsJsonAsync("/teams/1/roles", createTeamRoleCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongTeamId()
        {
            var createTeamRoleCommand = new CreateTeamRoleCommand()
            {
                TeamId = 0,
                Name = "Test role",
                PermissionIds = new List<int>() { 1, 2, 4, 6 }
            };

            var response = await _client.PostAsJsonAsync("/teams/1/roles", createTeamRoleCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongPermissionIds()
        {
            var createTeamRoleCommand = new CreateTeamRoleCommand()
            {
                TeamId = 1,
                Name = "Test role",
                PermissionIds = new List<int>() { 0, -1 }
            };

            var response = await _client.PostAsJsonAsync("/teams/1/roles", createTeamRoleCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var createTeamRoleCommand = new CreateTeamRoleCommand()
            {
                TeamId = 1,
                Name = "Test role",
                PermissionIds = new List<int>() { 1, 2, 4, 6 }
            };

            var response = await _unauthorizedClient.PostAsJsonAsync("/teams/1/roles", createTeamRoleCommand);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        private Team CreateTeamWithSeededRoles(IApplicationDbContext context)
        {
            var team = new Team()
            {
                Name = "Name",
                CreatorUserId = _currentUserId,
                Roles = context.Role.ToList(),
                Profiles = new List<TeamProfile>()
            };
            team.Profiles.Add(new TeamProfile()
            {
                UserId = _currentUserId,
                Team = team,
                RoleId = 1
            });
            return team;
        }
    }
}