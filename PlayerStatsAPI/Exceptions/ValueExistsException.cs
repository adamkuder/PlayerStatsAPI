﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerStatsAPI.Expression
{
    public class ValueExistsException : Exception
    {
        public ValueExistsException(string message) : base(message)
        {

        }
    }
}
