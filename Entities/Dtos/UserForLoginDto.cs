using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;

namespace Entities.Dtos
{
    public class UserForLoginDto:IDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int Type { get; set; }//1 Gönüllü, 2 STK, 3 Firma, 4 Okul,5 Admin
    }
}
