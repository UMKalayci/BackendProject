﻿using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;

namespace Entities.Dtos
{
    public class UserMailAuthDto : IDto
    {
        public string Email { get; set; }
    }
}
