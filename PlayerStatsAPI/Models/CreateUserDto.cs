using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerStatsAPI.Models
{
    public class CreateUserDto
    {

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string? Location { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int RoleId { get; set; }
    }
}
