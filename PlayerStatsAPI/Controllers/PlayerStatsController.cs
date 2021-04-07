using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayerStatsAPI.Controllers.Models;
using PlayerStatsAPI.Entities;
using PlayerStatsAPI.Models;
using PlayerStatsAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerStatsAPI.Controllers
{
    [Route("api/playerstats")]
    [ApiController]
    public class PlayerStatsController : ControllerBase
    {
        private readonly PlayerStatsDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPlayerStatsService _playerStatsService;

        public PlayerStatsController(IPlayerStatsService playerStatsService)
        {
            _playerStatsService = playerStatsService;
        }

        [HttpPost]
        [Route("Game")]
        public ActionResult CreateGame([FromBody]CreateGameDto dto)
        {
            var id = _playerStatsService.CreateGame(dto);

            return Created($"/api/playerStats/Game/{id}", null);
        }

        [HttpPost]
        [Route("User")]
        public ActionResult CreateUser([FromBody] CreateUserDto dto)
        {
            var id = _playerStatsService.CreateUser(dto);

            return Created($"/api/PlayerStats/User/{id}", null);
        }

        [HttpPost]
        public ActionResult CreatePlayerStats([FromBody] CreatePlayerStatsDto dto)
        {
            var id = _playerStatsService.CreatePlayerStats(dto);

            return Created($"/api/PlayerStats/{id}", null);
        }

        [HttpDelete("{id}")]
        //[Route("Game")]
        public ActionResult DeleteGame([FromRoute] int id)
        {
            _playerStatsService.DeleteGame(id);

            return NotFound();
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlayerStatsDto>> GetAll()
        {
            var playerStatsDto = _playerStatsService.GetAll();

            return Ok(playerStatsDto);
        }
        
        [HttpGet("{id}")]
        public ActionResult<PlayerStatsDto> Get([FromRoute] int id)
        {
            var users = _playerStatsService.GetById(id);

            return Ok(users);
        }
        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] UpdatePlayerStatsDto dto, [FromRoute]int id)
        {
            _playerStatsService.Update(id, dto);

            return Ok();
        }
    }
}
