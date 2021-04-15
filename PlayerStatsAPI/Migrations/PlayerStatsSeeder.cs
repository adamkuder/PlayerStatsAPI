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
                /*if(!_dbContext.PlayerStats.Any())
                {
                    var playerStats = GetPlayerStats();
                    _dbContext.PlayerStats.AddRange(playerStats);
                    _dbContext.SaveChanges();
                }*/
                if(!_dbContext.Role.Any())
                {
                    var role = GetRole();
                    _dbContext.Role.AddRange(role);
                    _dbContext.SaveChanges();
                }
                /*
                if(!_dbContext.User.Any())
                {
                    var user = GetUser();
                    _dbContext.User.AddRange(user);
                    _dbContext.SaveChanges();
                }*/
            }
        }

        private IEnumerable<Role> GetRole()
        {
            var role = new List<Role>
            {
                new Role()
                    {
                        Name = "Guest"
                    },
                new Role()
                    {
                        Name = "Register"
                    },
                new Role()
                    {
                        Name = "Moderator"
                    },
                new Role()
                    {
                        Name = "Administrator"
                    }
            };
            return role;
        }

        private IEnumerable<PlayerStats> GetPlayerStats()
        {
            var playerStats = new List<PlayerStats>
            {

            };
            return playerStats;
        }            
        
        private IEnumerable<User> GetUser()
        {
            var user = new List<User>
            {
                new User()
                    {
                        Name = "Adam",
                        EmailAddress = "adamkuder@mail.com",
                        Password = "jakieshaslo",

                    },
                new User()
                    {
                        Name = "Zbysiu",
                        EmailAddress = "xzbysiux@mail.com",
                        Password = "seridzem",
                    }
            };
            return user;
        }
    }
}
