using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Models.Chats.Dto;
using Models.Chats.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Chats.Queries
{
    public class GetChatByIdQueryHandler : IRequestHandler<GetChatByIdQuery, ChatDto>
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetChatByIdQueryHandler(
            IIdentityService identityService,
            IMapper mapper)
        {
            _identityService = identityService;
            _mapper = mapper;
        }

        public async Task<ChatDto> Handle(GetChatByIdQuery request, CancellationToken cancellationToken)
        {
            var currentUserChatProfile = await _identityService.GetCurrentUserChatProfileAsync(request.ChatId);
            var chatDto = _mapper.Map<ChatDto> (currentUserChatProfile.Chat);
            return chatDto;
        }
    }
}
