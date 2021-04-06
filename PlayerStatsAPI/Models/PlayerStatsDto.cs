using PlayerStatsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerStatsAPI.Controllers.Models
{
    public class PlayerStatsDto
    {
        public int Id { get; set; }
        public double Hours { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string NameGame { get; set; }
    }
}
