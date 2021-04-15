using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerStatsAPI.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        
        public string Password { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Location { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public virtual List<PlayerStats> PlayerStats { get; set; }
    }
}
