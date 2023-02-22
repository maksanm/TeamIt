using Application.Common.Exceptions;
using Azure.Core;
using Domain.Entities.Chats;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Chats.Commands;
using Models.Chats.Dto;
using Models.Chats.Queries;
using Models.Messages.Commands;
using Models.Messages.Dto;
using Models.Messages.Queries;
using Models.Permissions.Dto;
using Models.Projects.Commands;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [Route("chats")]
    [Authorize]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChatsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<ChatInfoDto>>> GetCurrentUserChatInfos()
        {
            IList<ChatInfoDto> chatInfoDtos;
            try
            {
                chatInfoDtos = await _mediator.Send(new GetCurrentUserChatInfosQuery());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(chatInfoDtos);
        }

        [HttpGet("{chatId}")]
        public async Task<ActionResult<ChatDto>> GetChatById([FromRoute] long chatId)
        {
            ChatDto chatDto;
            try
            {
                chatDto = await _mediator.Send(new GetChatByIdQuery() { ChatId = chatId});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(chatDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateChat([FromForm] CreateChatCommand request)
        {
            try
            {
                await _mediator.Send(request);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpPut("{chatId}")]
        public async Task<ActionResult> EditChat([FromForm] EditChatCommand request)
        {
            try
            {
                await _mediator.Send(request);
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpDelete("{chatId}")]
        public async Task<ActionResult> DeleteChat([FromRoute] long chatId)
        {
            try
            {
                await _mediator.Send(new DeleteChatCommand() { ChatId = chatId });
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
        
        [HttpPut("{chatId}/leave")]
        public async Task<ActionResult> LeaveChat(LeaveChatCommand request)
        {
            try
            {
                await _mediator.Send(request);
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpGet("users")]
        public async Task<ActionResult<UsersToChatWithDto>> GetUsersToChatWith()
        {
            UsersToChatWithDto dto;
            try
            {
                dto = await _mediator.Send(new GetUsersToChatWithQuery());
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(dto);
        }

        [HttpGet("{chatId}/users")]
        public async Task<ActionResult<UsersToChatWithDto>> GetUsersToAddToChat([FromRoute] long chatId)
        {
            UsersToChatWithDto dto;
            try
            {
                dto = await _mediator.Send(new GetUsersToAddToChatQuery() { ChatId = chatId });
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(dto);
        }

        [HttpPost("{chatId}/users")]
        public async Task<ActionResult> AddUserToChat(AddUserToChatCommand request)
        {
            try
            {
                await _mediator.Send(request);
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpGet("{chatId}/messages/{limit}")]
        public async Task<ActionResult<List<MessageDto>>> GetChatMessages([FromRoute] long chatId, [FromRoute] int limit = 50)
        {
            IList<MessageDto> messageDtos;
            try
            {
                messageDtos = await _mediator.Send(new GetChatMessagesQuery() { ChatId = chatId, Limit = limit });
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(messageDtos);
        }

        [HttpPost("{chatId}/messages")]
        public async Task<ActionResult> SendMessageToChat([FromForm] SendMessageCommand request)
        {
            try
            {
                await _mediator.Send(request);
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpGet("{chatId}/permissions")]
        public async Task<ActionResult<IList<PermissionDto>>> GetCurrentUserChatPermissions([FromRoute] long chatId)
        {
            IList<PermissionDto> permissionDtos;
            try
            {
                permissionDtos = await _mediator.Send(new GetCurrentUserChatPermissionsQuery() { ChatId = chatId });
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(permissionDtos);
        }
    }
}
