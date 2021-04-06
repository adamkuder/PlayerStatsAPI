using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerStatsAPI.Models
{
    public class UpdatePlayerStatsDto
    {
        [Required]
        public double Hours { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int GameId { get; set; }
    }
}
