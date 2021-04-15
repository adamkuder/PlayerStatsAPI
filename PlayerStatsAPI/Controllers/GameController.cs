using Microsoft.AspNetCore.Mvc;
using PlayerStatsAPI.Models;
using PlayerStatsAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerStatsAPI.Controllers
{
    [Route("api/Game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService _gameService)
        {
            _gameService = _gameService;
        }
        [HttpPost]
        public ActionResult Create([FromBody] CreateGameDto dto)
        {
            var id = _gameService.Create(dto);

            return Created($"/api/Game/{id}", null);
        }
        [HttpGet]
        public ActionResult<IEnumerable<GameDto>> GetAll([FromQuery] PlayerStatsQuery query)
        {
            var playerStatsDto = _gameService.GetAll(query);

            return Ok(playerStatsDto);
        }
    }
}
