using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Moq;
using Application.Common.Providers;
using Infrastructure.Providers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UnitTests.Providers
{
    public class PermissionsProviderTests
    {
        private IPermissionsProvider _permissionsProvider;
        private Mock<IApplicationDbContext> _contextMock;

        private IQueryable<Permission> _permissions;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            SetupEntities();
            SetupMocks();
            _permissionsProvider = new PermissionsProvider(_contextMock.Object);
        }

        [Test]
        public void ShouldReturnAllPermissions_AllPermissions()
        {
            var allPermssionsReturned = _permissionsProvider
                .AllPermissions()
                .All(res => _permissions
                    .Any(p => p.Id == res.Id));
            Assert.That(allPermssionsReturned, Is.True);
        }

        [Test]
        public void ShouldReturnAllPermissions_TeamCreator()
        {
            var allPermssionsReturned = _permissionsProvider
                .AllPermissions()
                .All(res => _permissions
                    .Any(p => p.Id == res.Id));
            Assert.That(allPermssionsReturned, Is.True);
        }

        [Test]
        [TestCase(new int[] { 1, 2, 3 })]
        [TestCase(new int[] { 14, 10, 6 })]
        [TestCase(new int[] { })]
        public void ShouldReturnPermissionsWithSpecifiedIds(int[] permissionIds)
        {
            var result = _permissionsProvider.PermissionsWithIds(permissionIds.ToList());
            var allSpecifiedPermssionsReturned = result
                .All(res => permissionIds
                    .Any(permId => permId == (int)res.Id));
            Assert.That(result.Count(), Is.EqualTo(permissionIds.Count()));
            Assert.That(allSpecifiedPermssionsReturned, Is.True);
        }

        private void SetupMocks()
        {
            _contextMock = new Mock<IApplicationDbContext>();
            var permissionsMock = new Mock<DbSet<Permission>>();

            permissionsMock.As<IQueryable<Permission>>().Setup(m => m.Provider).Returns(_permissions.Provider);
            permissionsMock.As<IQueryable<Permission>>().Setup(m => m.Expression).Returns(_permissions.Expression);
            permissionsMock.As<IQueryable<Permission>>().Setup(m => m.ElementType).Returns(_permissions.ElementType);
            permissionsMock.As<IQueryable<Permission>>().Setup(m => m.GetEnumerator()).Returns(() => _permissions.GetEnumerator());
            _contextMock
                .Setup(context => context.Permission)
                .Returns(permissionsMock.Object);
        }

        private void SetupEntities()
        {
            _permissions = new List<Permission>()
            {
                new Permission() { Id = PermissionEnum.TEAM_EDIT },
                new Permission() { Id = PermissionEnum.TEAM_DELETE },
                new Permission() { Id = PermissionEnum.TEAM_ADD_USER },
                new Permission() { Id = PermissionEnum.TEAM_KICK_USER },
                new Permission() { Id = PermissionEnum.TEAM_ASSIGN_ROLE },
                new Permission() { Id = PermissionEnum.TEAM_MANAGE_ROLE },
                new Permission() { Id = PermissionEnum.TEAM_CREATE_PROJECT },
                new Permission() { Id = PermissionEnum.TEAM_ADD_TO_PROJECT },
                new Permission() { Id = PermissionEnum.TEAM_LEAVE_PROJECT },
                new Permission() { Id = PermissionEnum.TEAM_DELETE_PROJECT },
                new Permission() { Id = PermissionEnum.PM_EDIT },
                new Permission() { Id = PermissionEnum.PM_ADD_TEAM },
                new Permission() { Id = PermissionEnum.PM_KICK_TEAM },
                new Permission() { Id = PermissionEnum.PM_SET_LIMIT_ROLE },
                new Permission() { Id = PermissionEnum.PM_CREATE_TASK },
                new Permission() { Id = PermissionEnum.PM_ASSIGN_TASK },
                new Permission() { Id = PermissionEnum.PM_CREATE_SUBTASK },
                new Permission() { Id = PermissionEnum.PM_EDIT_TASK },
                new Permission() { Id = PermissionEnum.PM_DELETE_TASK }
            }.AsQueryable();
        }
    }
}