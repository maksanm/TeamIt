using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Models.Chats.Dto;
using Models.Messages.Dto;
using Models.Messages.Queries;

namespace Application.Handlers.Messages.Queries
{
    public class GetChatMessagesQueryHandler : IRequestHandler<GetChatMessagesQuery, IList<MessageDto>>
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetChatMessagesQueryHandler(
            IIdentityService identityService,
            IMapper mapper)
        {
            _identityService = identityService;
            _mapper = mapper;
        }

        public async Task<IList<MessageDto>> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);
            var currentUserChatProfile = await _identityService.GetCurrentUserChatProfileAsync(request.ChatId);
            var messageDtos = _mapper.Map<IList<MessageDto>>(currentUserChatProfile.Chat.Messages);
            return messageDtos;
        }

        private void ValidateRequest(GetChatMessagesQuery request)
        {
            if (request.Limit < 0)
                throw new ValidationException("Message limit cannot be the negative value");
        }
    }
}
