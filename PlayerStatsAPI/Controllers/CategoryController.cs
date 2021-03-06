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
    [Route("api/Game/Category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpPost("{name}")]
        public ActionResult Create([FromRoute] string name, [FromBody]CreateCategoryDto dto)
        {
            var id = _categoryService.Create(name, dto);

            return Created($"/api/Game/Category/{id}", null);
        }
        [HttpGet]
        public ActionResult<IEnumerable<CategoryDto>> GetAll()
        {
            var categoryDto = _categoryService.GetAll();

            return Ok(categoryDto);
        }

        [HttpGet("{id}")]
        public ActionResult<CategoryDto> Get([FromRoute] int id)
        {
            var users = _categoryService.GetById(id);

            return Ok(users);
        }
    }
}
