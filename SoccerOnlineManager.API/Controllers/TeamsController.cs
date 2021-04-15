using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerOnlineManager.API.Helpers;
using SoccerOnlineManager.Application.Commands.Team;
using SoccerOnlineManager.Application.Exceptions;
using SoccerOnlineManager.Application.Queries.Team;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SoccerOnlineManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("teams")]
    public class TeamsController : ControllerBase
    {
        private readonly IMediator _mediatr;
        private readonly IAppService _appService;

        public TeamsController(IMediator mediatr, IAppService appService)
        {
            _mediatr = mediatr;
            _appService = appService;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTeamCommand command)
        {
            if (_appService.UserId != id && !_appService.IsAdmin)
                throw new ApiException(HttpStatusCode.Forbidden);

            if (!_appService.IsAdmin && command.TransferBudget != null)
                throw new ApiException(HttpStatusCode.Forbidden, ExceptionCodes.OnlyAdminCanChangeValue);

            command = new UpdateTeamCommand(command.Name, command.Country, command.TransferBudget, id);
            await _mediatr.Send(command);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateTeamCommand command)
        {
            await _mediatr.Send(command);
            return CreatedAtAction(nameof(Get), null);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetTeamsQuery();
            var result = await _mediatr.Send(query);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromQuery] GetTeamWithPlayersQuery query)
        {
            var result = await _mediatr.Send(query);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromQuery] DeleteTeamCommand command)
        {
            await _mediatr.Send(command);
            return NoContent();
        }
    }
}
