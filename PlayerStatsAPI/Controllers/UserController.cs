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
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public ActionResult Create([FromBody] CreateUserDto dto)
        {
            var id = _userService.Create(dto);

            return Created($"/api/User/{id}", null);
        }
        [HttpGet("{userId}")]
        public ActionResult<UserDto> Get([FromRoute] int userId)
        {
            UserDto user = _userService.GetById(userId);

            return Ok(user);
        }
        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> GetAll()
        {
            IEnumerable<UserDto> user = _userService.GetAll();

            return Ok(user);
        }
    }
}
