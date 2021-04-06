using PlayerStatsAPI.Controllers.Models;
using PlayerStatsAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerStatsAPI.Models
{
    public class GameDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<PlayerStatsDto> PlayerStats { get; set; }
    }
}
