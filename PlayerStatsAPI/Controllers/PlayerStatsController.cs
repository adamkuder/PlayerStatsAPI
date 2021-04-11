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
        private readonly IPlayerStatsService _playerStatsService;

        public PlayerStatsController(IPlayerStatsService playerStatsService)
        {
            _playerStatsService = playerStatsService;
        }

        

        [HttpPost]
        public ActionResult Create([FromBody] CreatePlayerStatsDto dto)
        {
            var id = _playerStatsService.Create(dto);

            return Created($"/api/PlayerStats/{id}", null);
        }

        [HttpDelete("{id}")]
        //[Route("Game")]
        public ActionResult Delete([FromRoute] int id)
        {
            _playerStatsService.Delete(id);

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
