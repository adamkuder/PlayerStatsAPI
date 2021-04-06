using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerStatsAPI.Entities
{
    public class PlayerStats
    {
        public int Id { get; set; }
        public double Hours { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }        
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
        
    }
}
