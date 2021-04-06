using AutoMapper;
using PlayerStatsAPI.Controllers.Models;
using PlayerStatsAPI.Entities;
using PlayerStatsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerStatsAPI
{
    public class PlayerStatsMappingProfile : Profile
    {
        public PlayerStatsMappingProfile()
        {
            CreateMap<PlayerStats, PlayerStatsDto>()
                .ForMember(m => m.Name, c => c.MapFrom(s => s.User.Name))
                .ForMember(m => m.EmailAddress, c => c.MapFrom(s => s.User.EmailAddress))
                .ForMember(m => m.NameGame, c => c.MapFrom(s => s.Game.Name));

            CreateMap<Game, GameDto>();

            CreateMap<CreateGameDto, Game>()
                .ForMember(r => r.Category, c => c.MapFrom(dto => new Category() { Name = dto.CategoryName }));

            CreateMap<CreateUserDto, User>();

            CreateMap<CreatePlayerStatsDto, PlayerStats>();
                
        }
    }
}
