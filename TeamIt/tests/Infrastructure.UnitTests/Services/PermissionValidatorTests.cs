using Domain.Entities.ProjectManager;
using Domain.Entities.Teams;
using Domain.Entities;
using Infrastructure.Services;
using Moq;
using Domain.Enums;
using Application.Common.Interfaces;
using Application.Common.Exceptions;

namespace Infrastructure.UnitTests.Services
{
    public class PermissionValidatorTests
    {
        private IPermissionValidator _permissionValidator;
        private Mock<IIdentityService> _identityServiceMock;

        private TeamProfile _userTeamProfile;
        private TeamProfile _creatorUserTeamProfile;
        private ProjectProfile _currentUserProjectProfile;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            SetupEntities();
            SetupMocks();
            _permissionValidator = new PermissionValidator(_identityServiceMock.Object);
        }

        [SetUp]
        public void Setup()
        {
            SetupMocks();
        }

        [Test]
        public void ShouldNotThrowException_UserHasTeamPermission()
        {
            Assert.DoesNotThrowAsync(() =>
                _permissionValidator
                    .ValidateTeamPermission(0, PermissionEnum.TEAM_MANAGE_ROLE));
        }

        [Test]
        public void ShouldNotThrowException_UserIsTeamCreator()
        {
            _identityServiceMock
                .Setup(service => service.GetCurrentUserTeamProfileAsync(It.IsAny<long>()))
                .ReturnsAsync(_creatorUserTeamProfile);
            Assert.DoesNotThrowAsync(() =>
                _permissionValidator
                    .ValidateTeamPermission(0, PermissionEnum.TEAM_MANAGE_ROLE));
        }

        [Test]
        public void ShouldThrowLackOfPermissionsException_UserHasNoTeamPermission()
        {
            Assert.ThrowsAsync<LackOfPermissionsException>(() =>
                _permissionValidator
                    .ValidateTeamPermission(0, PermissionEnum.TEAM_DELETE));
        }

        [Test]
        public void ShouldNotThrowException_UserHasProjectPermission()
        {
            Assert.DoesNotThrowAsync(() =>
                _permissionValidator
                    .ValidateProjectManagerPermission(0, PermissionEnum.PM_ADD_TEAM));
        }

        [Test]
        public void ShouldThrowLackOfPermissionsException_UserHasNoProjectPermission()
        {
            Assert.ThrowsAsync<LackOfPermissionsException>(() =>
                _permissionValidator
                    .ValidateProjectManagerPermission(0, PermissionEnum.PM_DELETE_TASK));
        }

        private void SetupMocks()
        {
            _identityServiceMock = new Mock<IIdentityService>();
            _identityServiceMock
                .Setup(service => service.GetCurrentUserTeamProfileAsync(It.IsAny<long>()))
                .ReturnsAsync(_userTeamProfile);
            _identityServiceMock
                .Setup(service => service.GetCurrentUserProjectProfileAsync(It.IsAny<long>()))
                .ReturnsAsync(_currentUserProjectProfile);
        }

        private void SetupEntities()
        {
            var team = new Team()
            {
                CreatorUserId = "creatorId",
                Profiles = new List<TeamProfile>() { _userTeamProfile, _creatorUserTeamProfile }
            };
            _userTeamProfile = new TeamProfile()
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
                            Id = PermissionEnum.TEAM_MANAGE_ROLE
                        }
                    }
                }
            };
            _creatorUserTeamProfile = new TeamProfile()
            {
                UserId = "creatorId",
                Team = team
            };
            var project = new Project()
            {
                CreatorTeam = team,
                UseOwnHierarchy = true,
                LimitRole = new Role()
                {
                    Permissions = new List<Permission>()
                    {
                        new Permission()
                        {
                            Id = PermissionEnum.PM_ADD_TEAM
                        }
                    }
                }
            };
            _currentUserProjectProfile = new ProjectProfile()
            {
                Project = project,
                TeamProfile = _userTeamProfile
            };
        }
    }
}