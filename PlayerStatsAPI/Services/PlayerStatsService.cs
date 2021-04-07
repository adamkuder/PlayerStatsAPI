using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlayerStatsAPI.Controllers.Models;
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
    public interface IPlayerStatsService
    {
        int CreateGame(CreateGameDto dto);
        int CreatePlayerStats(CreatePlayerStatsDto dto);
        int CreateUser(CreateUserDto dto);
        IEnumerable<PlayerStatsDto> GetAll();
        PlayerStatsDto GetById(int id);
        void DeleteGame(int id);
        void Update(int id, UpdatePlayerStatsDto dto);
    }

    public class PlayerStatsService : IPlayerStatsService
    {
        private readonly PlayerStatsDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<PlayerStatsService> _logger;

        public PlayerStatsService(PlayerStatsDbContext dbContext, IMapper mapper, ILogger<PlayerStatsService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }
        public PlayerStatsDto GetById(int id)
        {
            var users = _dbContext
                .PlayerStats
                .Include(r => r.User)
                .Include(r => r.Game)
                .FirstOrDefault(r => r.Id == id);
            if (users is null) throw new NotFoundException("Playerstats not found");
            var result = _mapper.Map<PlayerStatsDto>(users);
            return result;
        }
        public IEnumerable<PlayerStatsDto> GetAll()
        {
            var playerStats = _dbContext
               .PlayerStats
               .Include(r => r.User)
               .Include(r => r.Game)
               .ToList();
            var playerStatsDto = _mapper.Map<List<PlayerStatsDto>>(playerStats);


            return playerStatsDto;
        }
        public int CreatePlayerStats(CreatePlayerStatsDto dto)
        {
            var playerStats = _mapper.Map<PlayerStats>(dto);
            _dbContext.PlayerStats.Add(playerStats);
            _dbContext.SaveChanges();
            return playerStats.Id;
        }
        public int CreateUser(CreateUserDto dto)
        {
            var user = _mapper.Map<User>(dto);
            _dbContext.User.Add(user);
            _dbContext.SaveChanges();
            return user.Id;
        }
        public int CreateGame(CreateGameDto dto)
        {
            var game = _mapper.Map<Game>(dto);
            _dbContext.Game.Add(game);
            _dbContext.SaveChanges();
            return game.Id;
        }
        public void DeleteGame(int id)
        {
            _logger.LogError($"Game with id: {id} Delete action invoked");
            var game = _dbContext
                .Game
                .FirstOrDefault(r => r.Id == id);
            if (game is null) throw new NotFoundException("Game not found");
            _dbContext.Game.Remove(game);
            _dbContext.SaveChanges();
        }
        public void Update(int id, UpdatePlayerStatsDto dto)
        {
            var playerStats = _dbContext
                .PlayerStats
                .FirstOrDefault(r => r.Id == id);
            if (playerStats is null) throw new NotFoundException("PlayerStats not found");

            playerStats.Hours = dto.Hours;
            playerStats.UserId = dto.UserId;
            playerStats.GameId = dto.GameId;

            _dbContext.SaveChanges();

        }
    }
}
