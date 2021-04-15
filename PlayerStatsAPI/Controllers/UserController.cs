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
        [HttpPost("register")]
        public ActionResult Create([FromBody] CreateUserDto dto)
        {
            _userService.Create(dto);

            return Ok();
        }
        [HttpPost("login")]
        public ActionResult Login([FromBody]LoginDto dto)
        {
            string token = _userService.GenerateJwt(dto);
            
            return Ok(token);
        }
        [HttpGet("{userId}")]
        public ActionResult<UserDto> Get([FromRoute] int userId)
        {
            UserDto user = _userService.GetById(userId);

            return Ok(user);
        }
        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> GetAll(string searchPhrase)
        {
            IEnumerable<UserDto> user = _userService.GetAll(searchPhrase);

            return Ok(user);
        }
    }
}
