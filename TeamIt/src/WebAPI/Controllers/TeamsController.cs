using Application.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Roles.Commands;
using Models.Roles.Dto;
using Models.Roles.Queries;
using Models.Teams.Commands;
using Models.Teams.Dto;
using Models.Teams.Queries;
using Models.Users.Commands;

namespace WebAPI.Controllers
{
    [Route("teams")]
    [Authorize]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeamsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<TeamDto>>> GetCurrentUserTeams()
        {
            IList<TeamDto> teamDtos;
            try
            {
                teamDtos = await _mediator.Send(new GetCurrentUserTeamsQuery());
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(teamDtos);
        }

        [HttpGet("{teamId}/get")]
        public async Task<ActionResult<TeamDto>> GetTeamById([FromRoute] long teamId)
        {
            TeamDto teamDto;
            try
            {
                teamDto = await _mediator.Send(new GetTeamByIdQuery() { TeamId = teamId });
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(teamDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateTeam([FromForm] CreateTeamCommand request)
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

        [HttpPut("{teamId}")]
        public async Task<ActionResult> EditTeam([FromForm] EditTeamCommand request)
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

        [HttpPut("{teamId}/leave")]
        public async Task<ActionResult> LeaveTeam([FromRoute] long teamId)
        {
            try
            {
                await _mediator.Send(new LeaveTeamCommand() { TeamId = teamId });
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpDelete("{teamId}")]
        public async Task<ActionResult> DeleteTeam([FromRoute] long teamId)
        {
            try
            {
                await _mediator.Send(new DeleteTeamCommand() { TeamId = teamId });
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpGet("{teamId}/members")]
        public async Task<ActionResult<List<TeamMemberDto>>> GetTeamMembers([FromRoute] long teamId)
        {
            IList<TeamMemberDto> memberDtos;
            try
            {
                memberDtos = await _mediator.Send(new GetTeamMembersQuery() { TeamId = teamId });
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(memberDtos);
        }

        [HttpPut("{teamId}/members")]
        public async Task<ActionResult> AssignTeamMemberRole(AssignTeamMemberRoleCommand request)
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

        [HttpDelete("{teamId}/members/{userId}")]
        public async Task<ActionResult> KickTeamMember([FromRoute] long teamId, [FromRoute] string userId)
        {
            try
            {
                await _mediator.Send(new KickTeamMemberCommand() { TeamId = teamId, UserId = userId });
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpGet("{teamId}/roles")]
        public async Task<ActionResult<List<RoleDto>>> GetTeamRoles([FromRoute] long teamId)
        {
            IList<RoleDto> roleDtos;
            try
            {
                roleDtos = await _mediator.Send(new GetTeamRolesQuery() { TeamId = teamId });
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(roleDtos);
        }

        [HttpGet("{teamId}/role")]
        public async Task<ActionResult<RoleDto>> GetCurrentUserTeamRole([FromRoute] long teamId)
        {
            RoleDto roleDto;
            try
            {
                roleDto = await _mediator.Send(new GetCurrentUserTeamRoleQuery() { TeamId = teamId });
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(roleDto);
        }

        [HttpPost("{teamId}/roles")]
        public async Task<ActionResult> CreateTeamRole(CreateTeamRoleCommand request)
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

        [HttpPut("{teamId}/roles")]
        public async Task<ActionResult> EditTeamRole(EditTeamRoleCommand request)
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

        [HttpDelete("{teamId}/roles/{roleId}")]
        public async Task<ActionResult<RoleDto>> DeleteTeamRole([FromRoute] long teamId, [FromRoute] long roleId)
        {
            try
            {
                await _mediator.Send(new DeleteTeamRoleCommand() { TeamId = teamId, RoleId = roleId });
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpPut("{teamId}/creator")]
        public async Task<ActionResult> ChangeTeamCreator(ChangeTeamCreatorCommand request)
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

        [HttpPost("{teamId}/members")]
        public async Task<ActionResult> SendJoinTeamRequest(SendJoinTeamRequestCommand request)
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

        [HttpPost("invites/{inviteId}/answer")]
        public async Task<ActionResult> AnswerJoinTeamRequest(AnswerJoinTeamRequestCommand request)
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

        [HttpGet("invites")]
        public async Task<ActionResult<List<JoinTeamRequestDto>>> GetCurrentUserTeamInvites()
        {
            IList<JoinTeamRequestDto> joinTeamRequestDtos;
            try
            {
                joinTeamRequestDtos = await _mediator.Send(new GetCurrentUserJoinTeamRequestsQuery());
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(joinTeamRequestDtos);
        }
    }
}