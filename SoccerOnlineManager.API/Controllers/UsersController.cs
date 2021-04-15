using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerOnlineManager.API.Helpers;
using SoccerOnlineManager.Application.Commands.User;
using SoccerOnlineManager.Application.Queries.Team;
using SoccerOnlineManager.Application.Queries.User;
using System;
using System.Threading.Tasks;

namespace SoccerOnlineManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediatr;
        private readonly IAppService _appService;

        public UsersController(IMediator mediatr, IAppService appService)
        {
            _mediatr = mediatr;
            _appService = appService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateUserCommand command)
        {
            var result = await _mediatr.Send(command);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserCommand command)
        {
            await _mediatr.Send(command);
            return CreatedAtAction(nameof(Get), null);
        }

        [HttpGet("me/team")]
        public async Task<IActionResult> GetMyTeamInfo()
        {
            var userId = _appService.UserId;
            var query = new GetTeamWithPlayersQuery { TeamId = userId };
            var result = await _mediatr.Send(query);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetUsersQuery();
            var result = await _mediatr.Send(query);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromQuery] GetUserQuery query)
        {
            var result = await _mediatr.Send(query);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateUserCommand command)
        {
            command = new UpdateUserCommand(id, command.Email);
            await _mediatr.Send(command);
            return NoContent();
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromQuery] DeleteUserCommand command)
        {
            await _mediatr.Send(command);
            return NoContent();
        }
    }
}
