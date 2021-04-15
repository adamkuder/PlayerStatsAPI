using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayerStatsAPI.Controllers.Models;
using PlayerStatsAPI.Entities;
using PlayerStatsAPI.Models;
using PlayerStatsAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            //int? userId = (int?)int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var id = _playerStatsService.Create(dto);

            return Created($"/api/PlayerStats/{id}", null);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult Delete([FromRoute] int id)
        {
            
            _playerStatsService.Delete(id);

            return NotFound();
        }

        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<PlayerStatsDto>> GetAll([FromQuery] string searchPhrase)
        {
            var playerStatsDto = _playerStatsService.GetAll();

            return Ok(playerStatsDto);
        }
        
        [HttpGet("{id}")]
        [Authorize(Policy = "HasPlayerStats")]
        public ActionResult<PlayerStatsDto> Get([FromRoute] int id)
        {
            var users = _playerStatsService.GetById(id);

            return Ok(users);
        }
        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] UpdatePlayerStatsDto dto, [FromRoute]int id)
        {
            _playerStatsService.Update(id, dto, User);

            return Ok();
        }
    }
}
