using Application.Common.Exceptions;
using Domain.Entities;
using Domain.Entities.ProjectManager;
using Domain.Entities.Teams;
using Infrastructure.Services;
using Moq;

namespace Infrastructure.UnitTests.Services
{
    public class IdentityServiceTests
    {
        private Mock<IdentityService> _identityServiceMock;

        private User _currentUser;
        private Team _team;
        private Project _project;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            SetupEntities();
            SetupMocks();
        }

        [Test]
        public void ShouldReturnTeamProfile()
        {
            var returnedProfile = _identityServiceMock
                .Object
                .GetCurrentUserTeamProfileAsync(_team.Id)
                .Result;
            Assert.That(returnedProfile.User.Id, Is.EqualTo(_currentUser.Id));
        }

        [Test]
        public void ShouldReturnProjectProfile()
        {
            var returnedProfile = _identityServiceMock
                .Object
                .GetCurrentUserProjectProfileAsync(_project.Id)
                .Result;
            Assert.That(returnedProfile.User.Id, Is.EqualTo(_currentUser.Id));
        }

        [Test]
        public void ShouldThrowInvalidArgument_UserIsNotMemberOfTeam()
        {
            _currentUser
                .TeamProfiles
                .Clear();
            Assert.ThrowsAsync<ValidationException>(() =>
                _identityServiceMock
                    .Object
                    .GetCurrentUserTeamProfileAsync(_team.Id));
        }

        [Test]
        public void ShouldThrowInvalidArgument_UserIsNotMemberOfProject()
        {
            _currentUser
                .TeamProfiles
                .First()
                .ProjectProfiles
                .Clear();
            Assert.ThrowsAsync<ValidationException>(() =>
                _identityServiceMock
                    .Object
                    .GetCurrentUserProjectProfileAsync(_project.Id));
        }

        [Test]
        public void ShouldThrowUnauthorizedForNullCurrentUser_CurrentUserIsNull()
        {
            _identityServiceMock
               .Setup(service => service.GetCurrentUserAsync())
               .ReturnsAsync((User)null);
            Assert.ThrowsAsync<UnauthorizedException>(() =>
                _identityServiceMock
                    .Object
                    .GetCurrentUserTeamProfileAsync(_team.Id));
            Assert.ThrowsAsync<UnauthorizedException>(() =>
                _identityServiceMock
                    .Object
                    .GetCurrentUserProjectProfileAsync(_project.Id));
        }

        private void SetupMocks()
        {
            _identityServiceMock = new Mock<IdentityService>();
            _identityServiceMock
                .Setup(service => service.GetCurrentUserAsync())
                .ReturnsAsync(_currentUser);
        }

        private void SetupEntities()
        {
            _currentUser = new User()
            {
                Id = "id",
                TeamProfiles = new List<TeamProfile>()
            };
            _team = new Team()
            {
                Id = 1,
                Profiles = new List<TeamProfile>()
            };
            var currentUserTeamProfile = new TeamProfile()
            {
                Team = _team,
                User = _currentUser,
                ProjectProfiles = new List<ProjectProfile>()
            };
            _team.Profiles.Add(currentUserTeamProfile);
            _currentUser.TeamProfiles.Add(currentUserTeamProfile);
            _project = new Project()
            {
                Id = 2,
                CreatorTeam = _team,
                Profiles = new List<ProjectProfile>()
            };
            var currentUserProjectProfile = new ProjectProfile()
            {
                Project = _project,
                TeamProfile = currentUserTeamProfile
            };
            _project.Profiles.Add(currentUserProjectProfile);
            currentUserTeamProfile.ProjectProfiles.Add(currentUserProjectProfile);
        }
    }
}