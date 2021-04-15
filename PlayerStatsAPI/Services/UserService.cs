using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PlayerStatsAPI.Entities;
using PlayerStatsAPI.Expression;
using PlayerStatsAPI.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PlayerStatsAPI.Services
{
    public interface IUserService
    {
        void Create(CreateUserDto dto);
        void Delete(int id);
        UserDto GetById(int userId);
        IEnumerable<UserDto> GetAll(string searchPhrase);
        string GenerateJwt(LoginDto dto);
    }
    public class UserService : IUserService
    {
        private readonly PlayerStatsDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AutheticationSettings _autheticationSettings;

        public UserService(PlayerStatsDbContext dbContext, IMapper mapper, ILogger<UserService> logger, IPasswordHasher<User> passwordHasher, AutheticationSettings autheticationSettings)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _passwordHasher = passwordHasher;
            _autheticationSettings = autheticationSettings;
        }

        public void Create(CreateUserDto dto)
        {
            var user = new User()
            {
                Name = dto.Name,
                EmailAddress = dto.EmailAddress,
                DateOfBirth = dto.DateOfBirth,
                Location = dto.Location,
                RoleId = dto.RoleId
            };
            var hashedPassword = _passwordHasher.HashPassword(user, dto.Password);
            user.Password = hashedPassword;
            _dbContext.User.Add(user);
            _dbContext.SaveChanges();
        }


        public string GenerateJwt(LoginDto dto)
        {
            var user = _dbContext.User.FirstOrDefault(u => u.EmailAddress == dto.EmailAddress);

            if (user is null)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
            if(result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Name}"),
                new Claim(ClaimTypes.Role, $"{user.Role}"),
                new Claim(ClaimTypes.DateOfBirth, user.DateOfBirth.Value.ToString("yyyy-MM-dd")),
                new Claim("Location", user.Location)
            };
            if (user.PlayerStats is not null)
            {
                claims.Add(
                    new Claim("PlayerStats", "HasPlayerStats")
                    );
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_autheticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_autheticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_autheticationSettings.JwtIssuer, _autheticationSettings.JwtIssuer,
                claims, expires: expires, signingCredentials: cred);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);

        }

        public IEnumerable<UserDto> GetAll(string searchPhrase)
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

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
