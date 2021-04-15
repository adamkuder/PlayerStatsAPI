using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlayerStatsAPI.Entities;
using PlayerStatsAPI.Expression;
using PlayerStatsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerStatsAPI.Services
{
    public interface ICategoryService
    {
        int Create(string name, CreateCategoryDto dto);
        void Delete(int id);
        CategoryDto GetById(int id);
        IEnumerable<CategoryDto> GetAll();
    }
    public class CategoryService : ICategoryService
    {
        private readonly PlayerStatsDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CategoryService(PlayerStatsDbContext dbContext, IMapper mapper, ILogger<Category> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }
        public int Create(string name, CreateCategoryDto dto)
        {
            var category = _dbContext.Category.FirstOrDefault(r => r.Name == name);
            if (category is not null)
                throw new ValueExistsException("Category has Exists");
            dto.Name = name;
            var categoryEntity = _mapper.Map<Category>(dto);

            _dbContext.Category.Add(categoryEntity);
            _dbContext.SaveChanges();

            return categoryEntity.Id;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CategoryDto> GetAll()
        {
            var category = _dbContext
               .Category
               .Include(r => r.Games)
               .ToList();
            var categoryDto = _mapper.Map<List<CategoryDto>>(category);


            return categoryDto;
        }

        public CategoryDto GetById(int id)
        {
            var category = _dbContext
               .Category
               .Include(r => r.Games)
               .FirstOrDefault(r => r.Id == id);
            if (category is null) throw new NotFoundException("Playerstats not found");
            var result = _mapper.Map<CategoryDto>(category);
            return result;
        }
    }
}
