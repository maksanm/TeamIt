using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.Chats;
using Domain.Entities.ProjectManager;
using Domain.Entities.Teams;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Chats.Commands;
using Models.Projects.Commands;
using Models.Teams.Commands;

namespace Application.Handlers.Teams.Commands
{
    public class DeleteTeamCommandHandler : IRequestHandler<DeleteTeamCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPermissionValidator _permissionValidator;
        private readonly IMediator _mediator;

        private Team? _team;

        public DeleteTeamCommandHandler(
            IApplicationDbContext context,
            IPermissionValidator permissionValidator,
            IMediator mediator)
        {
            _context = context;
            _permissionValidator = permissionValidator;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);
            if (request.ValidatePermissions)
                await _permissionValidator.ValidateTeamPermission(request.TeamId, PermissionEnum.TEAM_DELETE);

            await ClearTeamRelatedEntities();
            _context.Team.Remove(_team!);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private void ValidateRequest(DeleteTeamCommand request)
        {
            _team = _context.Team.Find(request.TeamId);
            if (_team is null)
                throw new ValidationException("Team with provided id does not exist");
        }

        private async System.Threading.Tasks.Task ClearTeamRelatedEntities()
        {
            var projectsCreatedByTeam = GetProjectsCreatedByTeam();
            for (var i = 0; i < projectsCreatedByTeam.Count(); i++)
                await _mediator.Send(new DeleteProjectCommand() { ProjectId = projectsCreatedByTeam.ElementAt(i).Id, ValidatePermissions = false });
            var chatsCreatedByTeam = GetChatsCreatedByTeam();
            for (var i = 0; i < chatsCreatedByTeam.Count(); i++)
                await _mediator.Send(new DeleteChatCommand() { ChatId = chatsCreatedByTeam.ElementAt(i).Id, ValidatePermissions = false });
            foreach (var role in _team!.Roles)
                if (!Enum.IsDefined(typeof(BasicRoleEnum), (int)role.Id))
                    _context.Role.Remove(role);
            _context.TeamProfile.RemoveRange(_team.Profiles);
        }

        private List<Project> GetProjectsCreatedByTeam() =>
            _team!.Profiles
                .SelectMany(tp => tp.ProjectProfiles)
                .Select(pp => pp.Project)
                .Where(project => project.CreatorTeamId == _team.Id)
                .Distinct()
                .ToList();
        
        private List<Chat> GetChatsCreatedByTeam() =>
            _team!.Profiles
                .SelectMany(tp => tp.ChatProfiles)
                .Select(cp => cp.Chat)
                .Where(chat => chat.BaseTeam?.Id == _team.Id)
                .Distinct()
                .ToList();
    }
}