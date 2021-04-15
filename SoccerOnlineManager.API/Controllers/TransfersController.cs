using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerOnlineManager.API.Helpers;
using SoccerOnlineManager.Application.Commands.Transfer;
using SoccerOnlineManager.Application.Queries.Transfer;
using System;
using System.Threading.Tasks;

namespace SoccerOnlineManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("transfers")]
    public class TransfersController : ControllerBase
    {
        private readonly IMediator _mediatr;
        private readonly IAppService _appService;

        public TransfersController(IMediator mediatr, IAppService appService)
        {
            _mediatr = mediatr;
            _appService = appService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTransferCommand command)
        {
            command = new CreateTransferCommand(command.PlayerId, command.Price, _appService.IsAdmin, _appService.UserId);
            await _mediatr.Send(command);
            return CreatedAtAction(nameof(Get), null);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetTransfersQuery query)
        {
            var result = await _mediatr.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromQuery] GetTransferQuery query)
        {
            var result = await _mediatr.Send(query);
            return Ok(result);
        }

        [HttpPost("{id}/buy")]
        public async Task<IActionResult> Buy([FromRoute] Guid id)
        {
            var command = new BuyPlayerCommand(id, _appService.UserId);
            await _mediatr.Send(command);
            return Ok();
        }
    }
}
