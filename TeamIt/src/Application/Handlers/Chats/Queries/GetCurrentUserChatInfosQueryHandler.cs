using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Chats.Dto;
using Models.Chats.Queries;
using Models.Projects.Dto;

namespace Application.Handlers.Chats.Queries
{
    public class GetCurrentUserChatInfosQueryHandler : IRequestHandler<GetCurrentUserChatInfosQuery, IList<ChatInfoDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetCurrentUserChatInfosQueryHandler(
            IApplicationDbContext context,
            IIdentityService identityService,
            IMapper mapper)
        {
            _context = context;
            _identityService = identityService;
            _mapper = mapper;
        }

        public async Task<IList<ChatInfoDto>> Handle(GetCurrentUserChatInfosQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await _identityService.GetCurrentUserAsync();
            if (currentUser is null)
                throw new UnauthorizedException();

            var currentUserChats = await _context.ChatProfile
                .Where(cp => cp.TeamProfile != null && cp.TeamProfile.UserId == currentUser.Id ||
                             cp.ProjectProfile != null && cp.ProjectProfile.TeamProfile.UserId == currentUser.Id)
                .Select(cp => cp.Chat)
                .Distinct()
                .ToListAsync();
            var chatInfoDtos = _mapper.Map<IList<ChatInfoDto>>(currentUserChats);
            return chatInfoDtos;
        }
    }
}
