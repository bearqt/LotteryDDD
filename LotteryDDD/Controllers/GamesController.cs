using LotteryDDD.Application;
using LotteryDDD.DTO;
using Microsoft.AspNetCore.Mvc;

namespace LotteryDDD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _service;
        public GamesController(IGameService service)
        {
            _service = service;  
        }

        [HttpPost("join/{userGuid}")]
        public IActionResult JoinGame(Guid userGuid)
        {
            var gameGuid = _service.AddUserToGame(userGuid);
            return Ok(gameGuid);
        }

        [HttpPost("move")]
        public IActionResult MakeMove(MoveInputModel input)
        {
            var score = _service.MakeMove(input.UserId, input.GameId, input.Numbers);
            return Ok($"You received ${score} points. Congrats!");
        }

        [HttpGet("info/{gameGuid}")]
        public IActionResult GetGameInfo(Guid gameGuid)
        {
            var gameInfo = _service.GetGameInfo(gameGuid);
            return Ok(gameInfo);
        }

        [HttpPost("leave/{userGuid}")]
        public async Task<IActionResult> LeaveGame(Guid userGuid)
        {
            await _service.LeaveGame(userGuid);
            return Ok();
        }

    }
}
