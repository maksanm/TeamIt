using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Providers;
using Domain.Entities.ProjectManager;
using Domain.Enums;
using MediatR;
using Models.Projects.Commands;

namespace Application.Handlers.Projects.Commands
{
    public class SetLimitRoleCommandHandler : IRequestHandler<SetLimitRoleCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPermissionValidator _permissionValidator;
        private readonly IPermissionsProvider _permissionsProvider;

        private Project? _project;

        public SetLimitRoleCommandHandler(
            IApplicationDbContext context,
            IPermissionValidator permissionValidator,
            IPermissionsProvider permissionsProvider)
        {
            _context = context;
            _permissionValidator = permissionValidator;
            _permissionsProvider = permissionsProvider;
        }

        public async Task<Unit> Handle(SetLimitRoleCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);
            await _permissionValidator.ValidateProjectManagerPermission(request.ProjectId, PermissionEnum.PM_SET_LIMIT_ROLE);

            _project!.LimitRole.Name = request.Name;
            _project.LimitRole.Permissions.Clear();
            _project.LimitRole.Permissions = _permissionsProvider
                .PermissionsWithIds(request.PermissionIds)
                .ToList();
            _project.UseOwnHierarchy = request.UseOwnHierarchy;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private async System.Threading.Tasks.Task ValidateRequest(SetLimitRoleCommand request)
        {
            await ValidateProject(request.ProjectId);
            ValidateName(request.Name);
        }

        private async System.Threading.Tasks.Task ValidateProject(long projectId)
        {
            _project = await _context.Project.FindAsync(projectId);
            if (_project is null)
                throw new ValidationException("Project with provided id does not exist");
        }

        private void ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ValidationException("Limit role name connot be empty");
        }
    }
}