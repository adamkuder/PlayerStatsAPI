using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerStatsAPI.Entities
{
    public class PlayerStatsDbContext: DbContext
    {
        private string _connectionString = "Server=DESKTOP-PCG6126\\SQLEXPRESS;Database=PlayerStatsDb;Trusted_Connection=True;";
        public DbSet<PlayerStats> PlayerStats { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Game> Game { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Role> Role { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(r => r.Name).IsRequired().HasMaxLength(25);
            modelBuilder.Entity<User>().Property(r => r.Password).IsRequired();
            modelBuilder.Entity<User>().Property(r => r.EmailAddress).IsRequired();
            modelBuilder.Entity<Role>().Property(r => r.Name).IsRequired();
            modelBuilder.Entity<Game>().Property(r => r.Name).IsRequired().HasMaxLength(25);
            modelBuilder.Entity<Category>().Property(r => r.Name).IsRequired();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
