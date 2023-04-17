using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Providers;
using Domain.Entities.ProjectManager;
using Domain.Enums;
using MediatR;
using Models.Tasks.Commands;
using System.Threading.Tasks;

namespace Application.Handlers.Tasks.Commands
{
    public class EditTaskCommandHandler : IRequestHandler<EditTaskCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPermissionValidator _permissionValidator;

        private Project? _project;
        private Domain.Entities.ProjectManager.Task? _task;

        public EditTaskCommandHandler(
            IApplicationDbContext context,
            IPermissionValidator permissionValidator)
        {
            _context = context;
            _permissionValidator = permissionValidator;
        }

        public async Task<Unit> Handle(EditTaskCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);
            await _permissionValidator.ValidateProjectManagerPermission(request.ProjectId, PermissionEnum.PM_EDIT_TASK);

            if (!string.IsNullOrWhiteSpace(request.Name))
                _task!.Name = request.Name;
            _task!.Description = request.Description ?? _task.Description;
            _task.StartDate = request.StartDate ?? _task.StartDate;
            _task.DeadLine = request.DeadLine ?? _task.DeadLine;
            _task.State = request.State is null ? _task.State : (TaskStateEnum)request.State;

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private async System.Threading.Tasks.Task ValidateRequest(EditTaskCommand request)
        {
            await ValidateProject(request.ProjectId);
            ValidateTask(request.TaskId);
            if (request.Name is not null)
                ValidateName(request.Name);
            if (request.DeadLine is not null)
                ValidateDeadLine(request);
        }

        private async System.Threading.Tasks.Task ValidateProject(long projectId)
        {
            _project = await _context.Project.FindAsync(projectId);
            if (_project is null)
                throw new ValidationException("Team with provided id does not exist");
        }

        private void ValidateTask(long taskId)
        {
            _task = _project!.GetTask(taskId);
            if (_task == default)
                throw new ValidationException("Task with provided id does not exist in the project");
        }

        private void ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ValidationException("Task name cannot be empty");
        }

        private void ValidateDeadLine(EditTaskCommand request)
        {
            if (request.StartDate is not null && request.DeadLine < request.StartDate)
                throw new ValidationException("Deadline date should be greater than start date");
            if (request.DeadLine is not null && request.DeadLine < DateTime.Now)
                throw new ValidationException("Invalid deadline date");
        }
    }
}