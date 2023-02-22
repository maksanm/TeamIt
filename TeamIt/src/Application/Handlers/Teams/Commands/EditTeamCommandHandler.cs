using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.Teams;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Teams.Commands;

namespace Application.Handlers.Teams.Commands
{
    public class EditTeamCommandHandler : IRequestHandler<EditTeamCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPermissionValidator _permissionValidator;
        private readonly IImageService _imageService;

        private Team? _team;

        public EditTeamCommandHandler(
            IApplicationDbContext context,
            IPermissionValidator permissionValidator,
            IImageService imageService)
        {
            _context = context;
            _permissionValidator = permissionValidator;
            _imageService = imageService;
        }

        public async Task<Unit> Handle(EditTeamCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);
            await _permissionValidator.ValidateTeamPermission(request.TeamId, PermissionEnum.TEAM_EDIT);

            _team!.Name = request.Name;
            await _context.SaveChangesAsync(cancellationToken);
            if (request.Image is not null)
                await _imageService.SetTeamPicture(_team.Id, request.Image);
            return Unit.Value;
        }

        private async Task ValidateRequest(EditTeamCommand request)
        {
            await ValidateTeam(request.TeamId);
            await ValidateTeamName(request.Name);
        }

        private async Task ValidateTeam(long teamId)
        {
            _team = await _context.Team.FindAsync(teamId);
            if (_team is null)
                throw new ValidationException("Team with provided id does not exist");
        }

        private async Task ValidateTeamName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ValidationException("Team name cannot be empty");
            if (_team!.Name == name)
                return;
            var alreadyExists = await _context.Team.AnyAsync(t => t.Name == name);
            if (alreadyExists)
                throw new ValidationException("Team with provided name already exists");
        }
    }
}