﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerStatsAPI.Models
{
    public class CategoryDto
    {
        public string Name { get; set; }
        public List<CategoryGameDto> Games {get; set;}
    }
}
