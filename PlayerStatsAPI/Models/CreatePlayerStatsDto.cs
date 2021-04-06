using PlayerStatsAPI.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerStatsAPI.Models
{
    public class CreatePlayerStatsDto
    {
        public int Id { get; set; }
        [Required]
        public double Hours { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int GameId { get; set; }
    }
}
