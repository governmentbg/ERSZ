﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERSZ.Api.Authentication
{
    public class ApiKeyModel
    {
        public string AppSecret { get; set; }
        public string UserData { get; set; }
    }
}
