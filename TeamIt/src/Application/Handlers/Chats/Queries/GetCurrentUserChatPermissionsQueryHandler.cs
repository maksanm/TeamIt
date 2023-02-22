using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Models.Chats.Queries;
using Models.Permissions.Dto;

namespace Application.Handlers.Chats.Queries
{
    public class GetCurrentUserChatPermissionsQueryHandler : IRequestHandler<GetCurrentUserChatPermissionsQuery, IList<PermissionDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetCurrentUserChatPermissionsQueryHandler(
            IApplicationDbContext context,
            IIdentityService identityService,
            IMapper mapper)
        {
            _context = context;
            _identityService = identityService;
            _mapper = mapper;
        }

        public async Task<IList<PermissionDto>> Handle(GetCurrentUserChatPermissionsQuery request, CancellationToken cancellationToken)
        {
            var currentUserChatProfile = await _identityService.GetCurrentUserChatProfileAsync(request.ChatId);

            var permissions = currentUserChatProfile.Role.Permissions;
            var permissionDtos = _mapper.Map<IList<PermissionDto>>(permissions);
            return permissionDtos;
        }
    }
}
