using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Enums;
using MediatR;
using Models.Tasks.Commands;
using Domain.Entities.ProjectManager;

namespace Application.Handlers.Tasks.Commands
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPermissionValidator _permissionValidator;

        private Project? _project;
        private Domain.Entities.ProjectManager.Task? _task;

        public DeleteTaskCommandHandler(
            IApplicationDbContext context,
            IPermissionValidator permissionValidator)
        {
            _context = context;
            _permissionValidator = permissionValidator;
        }

        public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);
            await _permissionValidator.ValidateProjectManagerPermission(request.ProjectId, PermissionEnum.PM_DELETE_TASK);

            _context.Task.Remove(_task!);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private async System.Threading.Tasks.Task ValidateRequest(DeleteTaskCommand request)
        {
            await ValidateProject(request.ProjectId);
            ValidateTask(request.TaskId);
        }

        private async System.Threading.Tasks.Task ValidateProject(long projectId)
        {
            _project = await _context.Project.FindAsync(projectId);
            if (_project is null)
                throw new ValidationException("Project with provided id does not exist");
        }

        private void ValidateTask(long taskId)
        {
            _task = _project!.GetTask(taskId);
            if (_task == default)
                throw new ValidationException("There is no task with provided id in the project");
        }
    }
}