using LotteryDDD.Application;
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
    }
}
