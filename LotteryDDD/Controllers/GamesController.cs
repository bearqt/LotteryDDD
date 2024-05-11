using LotteryDDD.Application;
using LotteryDDD.DTO;
using LotteryDDD.Features;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LotteryDDD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _service;
        private readonly IMediator _mediator;
        public GamesController(IGameService service, IMediator mediator)
        {
            _service = service;  
            _mediator = mediator;
        }

        [HttpPost("join/{userGuid}")]
        public IActionResult JoinGame(Guid userGuid)
        {
            var gameGuid = _mediator.Send(new AddUserToGameCommand(userGuid));
            return Ok(gameGuid);
        }

        [HttpPost("move")]
        public IActionResult MakeMove(MoveInputModel input)
        {
            var score = _mediator.Send(new MakeMoveCommand(input.UserId, input.GameId, input.Numbers));
            return Ok($"You received ${score} points. Congrats!");
        }

        [HttpGet("info/{gameGuid}")]
        public IActionResult GetGameInfo(Guid gameGuid)
        {
            var gameInfo = _mediator.Send(new GetGameInfoQuery(gameGuid));
            return Ok(gameInfo);
        }

        [HttpPost("leave/{userGuid}")]
        public async Task<IActionResult> LeaveGame(Guid userGuid)
        {
            var result = await _mediator.Send(new LeaveGameCommand(userGuid));
            return Ok(result);
        }

    }
}
