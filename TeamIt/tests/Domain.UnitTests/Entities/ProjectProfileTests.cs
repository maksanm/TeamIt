using Domain.Entities;
using Domain.Entities.ProjectManager;
using Domain.Entities.Teams;
using Domain.Enums;

namespace Domain.UnitTests.Entities
{
    public class ProjectProfileTests
    {
        private ProjectProfile _projectProfile;
        private Project _project;
        private Team _otherCreatorTeam;

        [SetUp]
        public void OneTimeSetup()
        {
            SetupEntities();
        }

        [Test]
        public void ShouldHasPermssionUsingOwnHierarchy_SetByTeamRoleAndLimitRole()
        {
            _project.UseOwnHierarchy = true;
            _project.CreatorTeam = _otherCreatorTeam;
            var hasAddTeamPermission = _projectProfile
                .Role
                .Permissions
                .Any(p => p.Id == PermissionEnum.PM_ADD_TEAM);
            Assert.IsTrue(hasAddTeamPermission);
        }

        [Test]
        public void ShouldHasNoPermssionUsingOwnHierarchy_SetByTeamRole()
        {
            _project.UseOwnHierarchy = true;
            _project.CreatorTeam = _otherCreatorTeam;
            var hasAddTeamPermission = _projectProfile
                .Role
                .Permissions
                .Any(p => p.Id == PermissionEnum.PM_ASSIGN_TASK);
            Assert.IsFalse(hasAddTeamPermission);
        }

        [Test]
        public void ShouldHasNoPermssionUsingOwnHierarchy_SetByLimitRole()
        {
            _project.UseOwnHierarchy = true;
            _project.CreatorTeam = _otherCreatorTeam;
            var hasAddTeamPermission = _projectProfile
                .Role
                .Permissions
                .Any(p => p.Id == PermissionEnum.PM_KICK_TEAM);
            Assert.IsFalse(hasAddTeamPermission);
        }

        [Test]
        public void ShouldHasPermssionNotUsingOwnHierarchy_SetByLimitRole()
        {
            _project.UseOwnHierarchy = false;
            _project.CreatorTeam = _otherCreatorTeam;
            var hasAddTeamPermission = _projectProfile
                .Role
                .Permissions
                .Any(p => p.Id == PermissionEnum.PM_KICK_TEAM);
            Assert.IsTrue(hasAddTeamPermission);
        }

        [Test]
        public void ShouldHasPermssionNotUsingOwnHierarchy_SetByTeamRoleOnly_CreatorTeam()
        {
            _project.UseOwnHierarchy = false;
            var hasAddTeamPermission = _projectProfile
                .Role
                .Permissions
                .Any(p => p.Id == PermissionEnum.PM_ASSIGN_TASK);
            Assert.IsTrue(hasAddTeamPermission);
        }

        [Test]
        public void ShouldHasNoPermssionNotUsingOwnHierarchy_SetByLimitRole_CreatorTeam()
        {
            _project.UseOwnHierarchy = false;
            var hasAddTeamPermission = _projectProfile
                .Role
                .Permissions
                .Any(p => p.Id == PermissionEnum.PM_KICK_TEAM);
            Assert.IsFalse(hasAddTeamPermission);
        }

        private void SetupEntities()
        {
            var team = new Team()
            {
                Id = 1,
                CreatorUserId = "creatorId",
                Profiles = new List<TeamProfile>()
            };
            var teamProfile = new TeamProfile()
            {
                UserId = "id",
                Team = team,
                Role = new Role()
                {
                    Permissions = new List<Permission>()
                    {
                        new Permission()
                        {
                            Id = PermissionEnum.PM_ADD_TEAM
                        },
                        new Permission()
                        {
                            Id = PermissionEnum.PM_ASSIGN_TASK
                        }
                    }
                }
            };
            team.Profiles.Add(teamProfile);
            _project = new Project()
            {
                CreatorTeam = team,
                LimitRole = new Role()
                {
                    Permissions = new List<Permission>()
                    {
                        new Permission()
                        {
                            Id = PermissionEnum.PM_ADD_TEAM
                        },
                        new Permission()
                        {
                            Id = PermissionEnum.PM_KICK_TEAM
                        }
                    }
                }
            };
            _projectProfile = new ProjectProfile()
            {
                Project = _project,
                TeamProfile = teamProfile
            };
            _otherCreatorTeam = new Team()
            {
                Id = 2,
                CreatorUserId = "otherCreatorId",
                Profiles = new List<TeamProfile>()
            };
        }
    }
}