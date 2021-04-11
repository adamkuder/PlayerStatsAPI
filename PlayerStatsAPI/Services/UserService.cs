using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlayerStatsAPI.Entities;
using PlayerStatsAPI.Exceptions;
using PlayerStatsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerStatsAPI.Services
{
    public interface IUserService
    {
        int Create(CreateUserDto dto);
        void Delete(int id);
        UserDto GetById(int userId);
        IEnumerable<UserDto> GetAll();
    }
    public class UserService : IUserService
    {
        private readonly PlayerStatsDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(PlayerStatsDbContext dbContext, IMapper mapper, ILogger<UserService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public int Create(CreateUserDto dto)
        {
            var user = _mapper.Map<User>(dto);
            _dbContext.User.Add(user);
            _dbContext.SaveChanges();
            return user.Id;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserDto> GetAll()
        {
            var users = _dbContext
               .User
               .Include(r => r.PlayerStats)
               .ToList();
            var playerStats = _dbContext
              .PlayerStats
              .Include(r => r.User)
              .ToList();
            var usersDto = _mapper.Map<List<UserDto>>(users);


            return usersDto;
        }

        public UserDto GetById(int userId)
        {
            var user = _dbContext
                .User
                .Include(r=> r.PlayerStats)                
                .FirstOrDefault(r => r.Id == userId);
            if (user is null)
                throw new NotFoundException("User Id not found");
            var listgame = user.PlayerStats;
            var userDto = _mapper.Map<UserDto>(user);
            //var playerstats = _mapper.Map<PlayerStatsByUserDto>(user);
            return userDto;
        }
    }
}
