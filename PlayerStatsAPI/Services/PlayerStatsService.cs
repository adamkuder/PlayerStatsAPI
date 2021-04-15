using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlayerStatsAPI.Authorization;
using PlayerStatsAPI.Controllers.Models;
using PlayerStatsAPI.Entities;
using PlayerStatsAPI.Expression;
using PlayerStatsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PlayerStatsAPI.Services
{
    public interface IPlayerStatsService
    {
        
        int Create(CreatePlayerStatsDto dto);
        public void Delete(int id);
        IEnumerable<PlayerStatsDto> GetAll();
        PlayerStatsDto GetById(int id);
        void Update(int id, UpdatePlayerStatsDto dto, ClaimsPrincipal user);
    }

    public class PlayerStatsService : IPlayerStatsService
    {
        private readonly PlayerStatsDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<PlayerStatsService> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public PlayerStatsService(PlayerStatsDbContext dbContext, IMapper mapper, ILogger<PlayerStatsService> logger,
            IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
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
        public int Create(CreatePlayerStatsDto dto)
        {
            var playerStats = _mapper.Map<PlayerStats>(dto);
            var user = _dbContext.User.FirstOrDefault(r => r.Id == _userContextService.GetUserId);
            
            if (user is null)
                throw new NotFoundException("User Id not found");
            var game = _dbContext.Game.FirstOrDefault(r => r.Id == playerStats.GameId);
            if (game is null)
                throw new NotFoundException("Game Id not found");

            _dbContext.PlayerStats.Add(playerStats);
            _dbContext.SaveChanges();
            return playerStats.Id;
        }
        
        
        public void Update(int id, UpdatePlayerStatsDto dto, ClaimsPrincipal user)
        {            
            var playerStats = _dbContext
                .PlayerStats
                .FirstOrDefault(r => r.Id == id);
            if (playerStats is null) throw new NotFoundException("PlayerStats not found");

            var authorizationResult = _authorizationService.AuthorizeAsync(user, playerStats, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if(!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            playerStats.Hours = dto.Hours;
            playerStats.UserId = dto.UserId;
            playerStats.GameId = dto.GameId;

            _dbContext.SaveChanges();

        }
        public void Delete(int id)
        {
            _logger.LogError($"PlayerStats with id: {id} Delete action invoked");
            var playerStats = _dbContext
                .PlayerStats
                .FirstOrDefault(r => r.Id == id);
            if (playerStats is null) throw new NotFoundException("PlayerStats not found");
            _dbContext.PlayerStats.Remove(playerStats);
            _dbContext.SaveChanges();
        }
    }
}
