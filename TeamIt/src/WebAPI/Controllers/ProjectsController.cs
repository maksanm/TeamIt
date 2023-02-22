using Application.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Permissions.Dto;
using Models.Projects.Commands;
using Models.Projects.Dto;
using Models.Projects.Queries;
using Models.Tasks.Commands;
using Models.Tasks.Dto;
using Models.Tasks.Queries;
using Models.Teams.Dto;

namespace WebAPI.Controllers
{
    [Route("projects")]
    [Authorize]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectInfoDto>>> GetCurrentUserProjectInfos()
        {
            IList<ProjectInfoDto> projectInfoDtos;
            try
            {
                projectInfoDtos = await _mediator.Send(new GetCurrentUserProjectInfosQuery());
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(projectInfoDtos);
        }

        [HttpGet("{projectId}")]
        public async Task<ActionResult<ProjectDto>> GetProjectById([FromRoute] long projectId)
        {
            ProjectDto projectDto;
            try
            {
                projectDto = await _mediator.Send(new GetProjectByIdQuery() { ProjectId = projectId });
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(projectDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProject([FromForm] CreateProjectCommand request)
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

        [HttpPut("{projectId}")]
        public async Task<ActionResult> EditProject([FromForm] EditProjectCommand request)
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

        [HttpPut("{projectId}/limitrole")]
        public async Task<ActionResult> SetProjectLimitRole(SetLimitRoleCommand request)
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

        [HttpPut("{projectId}/leave")]
        public async Task<ActionResult> LeaveProject(LeaveProjectCommand request)
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

        [HttpDelete("{projectId}")]
        public async Task<ActionResult> DeleteProject([FromRoute] long projectId)
        {
            try
            {
                await _mediator.Send(new DeleteProjectCommand() { ProjectId = projectId });
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpGet("{projectId}/permissions")]
        public async Task<ActionResult<List<PermissionDto>>> GetCurrentUserProjectPermissions([FromRoute] long projectId)
        {
            IList<PermissionDto> permissionDtos;
            try
            {
                permissionDtos = await _mediator.Send(new GetCurrentUserProjectPermissionsQuery() { ProjectId = projectId });
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(permissionDtos);
        }

        [HttpGet("{projectId}/members")]
        public async Task<ActionResult<ProjectMembersDto>> GetProjectMembers([FromRoute] long projectId)
        {
            ProjectMembersDto result;
            try
            {
                result = await _mediator.Send(new GetProjectMembersQuery() { ProjectId = projectId });
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(result);
        }

        [HttpGet("{projectId}/members/{name}")]
        public async Task<ActionResult<List<TeamDto>>> GetTeamsToAddToProject([FromRoute] long projectId, [FromRoute] string name)
        {
            IList<TeamDto> teamDtos;
            try
            {
                teamDtos = await _mediator.Send(new GetTeamsToAddToProjectQuery() { ProjectId = projectId, Name = name });
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(teamDtos);
        }

        [HttpPost("{projectId}/team")]
        public async Task<ActionResult> AddTeamToProject(AddTeamToProjectCommand request)
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

        [HttpDelete("{projectId}/team/{teamId}")]
        public async Task<ActionResult> KickTeamFromProject([FromRoute] long projectId, [FromRoute] long teamId)
        {
            try
            {
                await _mediator.Send(new KickTeamFromProjectCommand() { ProjectId = projectId , TeamId = teamId });
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpGet("{projectId}/tasks")]
        public async Task<ActionResult<List<TaskDto>>> GetProjectTasksQuery([FromRoute] long projectId)
        {
            IList<TaskDto> taskDtos;
            try
            {
                taskDtos = await _mediator.Send(new GetProjectTasksQuery() { ProjectId = projectId });
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(taskDtos);
        }

        [HttpGet("{projectId}/tasks/{taskId}")]
        public async Task<ActionResult<List<TaskDto>>> GetTaskByIdQuery([FromRoute] long projectId, [FromRoute] long taskId)
        {
            TaskDto taskDto;
            try
            {
                taskDto = await _mediator.Send(new GetTaskByIdQuery() { ProjectId = projectId, TaskId = taskId });
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(taskDto);
        }

        [HttpGet("{projectId}/tasks/current")]
        public async Task<ActionResult<List<TaskInfoDto>>> GetCurrentUserTaskInfos([FromRoute] long projectId)
        {
            IList<TaskInfoDto> taskInfoDtos;
            try
            {
                taskInfoDtos = await _mediator.Send(new GetCurrentUserTaskInfosQuery() { ProjectId = projectId });
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(taskInfoDtos);
        }

        [HttpPost("{projectId}/tasks")]
        public async Task<ActionResult> CreateTask(CreateTaskCommand request)
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

        [HttpPut("{projectId}/tasks/{taskId}")]
        public async Task<ActionResult> EditTask(EditTaskCommand request)
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

        [HttpPut("{projectId}/tasks/{taskId}/assign")]
        public async Task<ActionResult> AssignUserToTask(AssignUserToTaskCommand request)
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

        [HttpDelete("{projectId}/tasks/{taskId}")]
        public async Task<ActionResult> DeleteTask([FromRoute] long projectId, [FromRoute] long taskId)
        {
            try
            {
                await _mediator.Send(new DeleteTaskCommand() { ProjectId = projectId, TaskId = taskId });
            }
            catch (CustomApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
    }
}