using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlayerStatsAPI.Entities;
using PlayerStatsAPI.Expression;
using PlayerStatsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlayerStatsAPI.Services
{
    public interface IGameService
        {
            int Create(CreateGameDto dto);
            void Delete(int id);
            PageResult<GameDto> GetAll(PlayerStatsQuery query);
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

        public PageResult<GameDto> GetAll(PlayerStatsQuery query)
        {
            var baseQuery = _dbContext
                .Game
                .Include(r => r.Category)
                .Include(r => r.PlayerStats)
                .Where(r => query.SearchPhrase == null && r.Name.ToLower().Contains(query.SearchPhrase.ToLower()));
            if(!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelector = new Dictionary<string, Expression<Func<Game, object>>>
                {
                    { nameof(Game.Name), r => r.Name },
                    { nameof(Game.Category), r => r.Category },
                    { nameof(Game.PlayerStats), r => r.PlayerStats }
                };
                var selectedColumn = columnsSelector[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.ASC ? 
                    baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }
            var games = baseQuery
                .Skip(query.PageSize * (query.PageNumber-1))
                .Take(query.PageSize)
                .ToList();
            var totalItemsCount = baseQuery.Count();
            var gamesDto = _mapper.Map<List<GameDto>>(games);


            var result = new PageResult<GameDto>(gamesDto, totalItemsCount, query.PageSize, query.PageNumber);
            return result;
        }
    }
}

