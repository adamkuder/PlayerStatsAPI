using PlayerStatsAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerStatsAPI.Migrations
{
    public class PlayerStatsSeeder
    {
        private readonly PlayerStatsDbContext _dbContext;

        public PlayerStatsSeeder(PlayerStatsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {
            if(_dbContext.Database.CanConnect())
            {
                if(!_dbContext.PlayerStats.Any())
                {
                    var playerStats = GetPlayerStats();
                    _dbContext.PlayerStats.AddRange(playerStats);
                    _dbContext.SaveChanges();
                }              
            }
        }
        private IEnumerable<PlayerStats> GetPlayerStats()
        {
            var playerStats = new List<PlayerStats>
            {
                new PlayerStats()
                {
                    Hours = 80,
                    Game = new Game()
                    {
                        Name = "Contery",
                        Category = new Category()
                        {
                            Name = "Shooter"
                        }
                    },
                    User = new User()
                    {
                        Name = "Adam",
                        EmailAddress = "adamkuder@mail.com",
                        Password = "jakieshaslo",
                    }

                },
            new PlayerStats()
            {
                Hours = 23,
                Game = new Game()
                {
                    Name = "Civil",
                    Category = new Category()
                    {
                        Name = "strategy"
                    }
                },
                User = new User()
                    {
                        Name = "Zbysiu",
                        EmailAddress = "xzbysiux@mail.com",
                        Password = "seridzem",
                    }
            }
        };
            
        return playerStats;
            
        }
    }
}
