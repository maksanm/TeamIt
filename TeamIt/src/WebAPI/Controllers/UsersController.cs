using Application.Common.Exceptions;
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Teams.Dto;
using Models.Users.Commands;
using Models.Users.Dto;
using Models.Users.Queries;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> RegisterUser([FromForm] RegisterCommand registerCommand)
        {
            string token;
            try
            {
                token = await _mediator.Send(registerCommand);
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(token);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUser(LoginCommand loginCommand)
        {
            string token;
            try
            {
                token = await _mediator.Send(loginCommand);
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(token);
        }

        [HttpGet("{name}"), Authorize]
        public async Task<ActionResult<List<UserDto>>> GetUsersWithName([FromRoute] string name)
        {
            IList<UserDto> userDtos;
            try
            {
                userDtos = await _mediator.Send(new GetUsersWithNameQuery() { Name = name });
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(userDtos);
        }

        [HttpGet("current"), Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            UserDto userDto;
            try
            {
                userDto = await _mediator.Send(new GetCurrentUserQuery());
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(userDto);
        }
    }
}