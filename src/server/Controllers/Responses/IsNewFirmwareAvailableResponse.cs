﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimbait.Server.Controllers.Responses
{
    public class IsNewFirmwareAvailableResponse : BasicResponse
    {
        public bool available { get; set; }
    }
}
