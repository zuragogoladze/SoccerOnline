using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerOnlineManager.API.Helpers;
using SoccerOnlineManager.Application.Commands.Player;
using SoccerOnlineManager.Application.Exceptions;
using SoccerOnlineManager.Application.Queries.Player;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SoccerOnlineManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("players")]
    public class PlayersController : ControllerBase
    {
        private readonly IMediator _mediatr;
        private readonly IAppService _appService;

        public PlayersController(IMediator mediatr, IAppService appService)
        {
            _mediatr = mediatr;
            _appService = appService;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatePlayerCommand command)
        {
            if (!_appService.IsAdmin && command.MarketValue != null)
                throw new ApiException(HttpStatusCode.Forbidden, ExceptionCodes.OnlyAdminCanChangeValue);

            command = new UpdatePlayerCommand(id, command.FirstName, command.LastName, command.Country, command.MarketValue,
                                              _appService.UserId, _appService.IsAdmin);

            await _mediatr.Send(command);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePlayerCommand command)
        {
            await _mediatr.Send(command);
            return CreatedAtAction(nameof(Get), null);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetPlayersQuery();
            var result = await _mediatr.Send(query);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromQuery] GetPlayerQuery query)
        {
            var result = await _mediatr.Send(query);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromQuery] DeletePlayerCommand command)
        {
            await _mediatr.Send(command);
            return NoContent();
        }
    }
}
