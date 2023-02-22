using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.Teams;
using Infrastructure.Persistance;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace Application.IntegrationTests.Roles.Commands
{
    public class DeleteTeamRoleCommandTests : TestsBase
    {
        [OneTimeSetUp]
        public void OneTimeSetUp_DeleteTeamRoleCommandTests()
        {
            var context = GetDbContext();
            context.Team.Add(CreateTeam(context));
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
        public async Task ShouldDeleteTeamRole()
        {
            var response = await _client.DeleteAsync("/teams/1/roles/3");

            var context = GetDbContext();
            var teamRolesCountDb = context.Team.First().Roles.Count;
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(teamRolesCountDb, Is.EqualTo(2));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongTeamId()
        {
            var response = await _client.DeleteAsync("/teams/0/roles/3");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnBadRequest_ForWrongRoleId()
        {
            var response = await _client.DeleteAsync("/teams/1/roles/0");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnUnauthorized()
        {
            var response = await _unauthorizedClient.DeleteAsync("/teams/1/roles/3");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        private Team CreateTeam(IApplicationDbContext context)
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

        private Role CreateRole(ApplicationDbContext context) =>
            new Role()
            {
                Id = 3,
                Name = "Test role",
                Permissions = context.Permission.Take(3).ToList()
            };
    }
}