﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSBI.Test.Application.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string msg) : base(msg) {}
    }
}
