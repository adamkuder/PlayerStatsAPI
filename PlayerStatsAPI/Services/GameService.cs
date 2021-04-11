using AutoMapper;
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
    public interface IGameService
        {
            int Create(CreateGameDto dto);
            void Delete(int id);
        }
    public class GameService : IGameService
    {
        private readonly PlayerStatsDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public GameService(PlayerStatsDbContext dbContext, IMapper mapper, ILogger<GameService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }
        public int Create(CreateGameDto dto)
        {
            var gameEntity = _mapper.Map<Game>(dto);
            _dbContext.Game.Add(gameEntity);
            _dbContext.SaveChanges();
            return gameEntity.Id;
        }
        public void Delete(int id)
        {
            _logger.LogError($"Game with id: {id} Delete action invoked");
            var game = _dbContext
                .Game
                .FirstOrDefault(r => r.Id == id);
            if (game is null) throw new NotFoundException("Game not found");
            _dbContext.Game.Remove(game);
            _dbContext.SaveChanges();
        }
    }
}

