using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Permissions.Dto;
using Models.Permissions.Queries;

namespace WebAPI.Controllers
{
    [Route("permissions")]
    [Authorize]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<PermissionDto>>> GetPermissions()
        {
            var permissionDtos = await _mediator.Send(new GetPermissionsQuery());
            return Ok(permissionDtos);
        }
    }
}