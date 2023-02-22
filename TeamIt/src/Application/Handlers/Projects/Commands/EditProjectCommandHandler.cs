using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Providers;
using Domain.Entities.ProjectManager;
using Domain.Enums;
using MediatR;
using Models.Projects.Commands;

namespace Application.Handlers.Projects.Commands
{
    public class EditProjectCommandHandler : IRequestHandler<EditProjectCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPermissionValidator _permissionValidator;
        private readonly IImageService _imageService;

        private Project? _project;

        public EditProjectCommandHandler(
            IApplicationDbContext context,
            IPermissionValidator permissionValidator,
            IImageService imageService)
        {
            _context = context;
            _permissionValidator = permissionValidator;
            _imageService = imageService;
        }

        public async Task<Unit> Handle(EditProjectCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);
            await _permissionValidator.ValidateProjectManagerPermission(request.ProjectId, PermissionEnum.PM_EDIT);

            _project!.Name = request.Name ?? _project.Name;
            _project.Description = request.Description ?? _project.Description;
            _project.StartDate = request.StartDate ?? _project.StartDate;
            _project.DeadLine = request.DeadLine ?? _project.DeadLine;
            await _context.SaveChangesAsync(cancellationToken);
            if (request.Image is not null)
                await _imageService.SetProjectPicture(_project.Id, request.Image);
            return Unit.Value;
        }

        private async System.Threading.Tasks.Task ValidateRequest(EditProjectCommand request)
        {
            await ValidateProject(request.ProjectId);
            ValidateProjectName(request.Name!);
            if (request.DeadLine is not null)
                ValidateDeadLine(request);
        }

        private async System.Threading.Tasks.Task ValidateProject(long projectId)
        {
            _project = await _context.Project.FindAsync(projectId);
            if (_project is null)
                throw new ValidationException("Project with provided id does not exist");
        }

        private static void ValidateProjectName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Project name cannot be empty");
        }

        private void ValidateDeadLine(EditProjectCommand request)
        {
            if (request.StartDate is not null && request.DeadLine < request.StartDate)
                throw new ValidationException("Deadline date should be greater than start date");
            if (request.DeadLine < DateTime.Now)
                throw new ValidationException("Invalid deadline date");
        }
    }
}