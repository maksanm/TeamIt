using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Services
{
    public class PermissionValidator : IPermissionValidator
    {
        private readonly IIdentityService _identityService;

        public PermissionValidator(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task ValidateTeamPermission(long teamId, PermissionEnum permission)
        {
            var currentUserTeamProfile = await _identityService.GetCurrentUserTeamProfileAsync(teamId);
            var isTeamCreator = currentUserTeamProfile.UserId == currentUserTeamProfile.Team.CreatorUserId;
            if (!HasPermission(currentUserTeamProfile, permission) && !isTeamCreator)
                throw new LackOfPermissionsException($"Lack of permission: {permission}");
        }

        public async Task ValidateProjectManagerPermission(long projectId, PermissionEnum permission)
        {
            var currentUserProjectProfile = await _identityService.GetCurrentUserProjectProfileAsync(projectId);
            if (!HasPermission(currentUserProjectProfile, permission))
                throw new LackOfPermissionsException($"Lack of permission: {permission}");
        }

        public async Task ValidateChatPermission(long chatId, PermissionEnum permission)
        {
            var currentUserChatProfile = await _identityService.GetCurrentUserChatProfileAsync(chatId);
            if (!HasPermission(currentUserChatProfile, permission))
                throw new LackOfPermissionsException($"Lack of permission: {permission}");
        }

        private bool HasPermission(IProfile profile, PermissionEnum permission) =>
            profile.Role
                .Permissions
                .Any(p => p.Id == permission);
    }
}