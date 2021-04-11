using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerStatsAPI.Models
{
    public class PlayerStatsByUserDto
    {
        //public int Id { get; set; }
        public double Hours { get; set; }
        public int UserId { get; set; }
        //public int GameId { get; set; }
    }
}
