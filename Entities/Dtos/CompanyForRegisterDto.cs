using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class CompanyForRegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public DateTime FoundationOfYear { get; set; }
        public string Phone { get; set; }
        public int CityId { get; set; }
        public byte[] Image { get; set; }
        public string Desc { get; set; }
    }
}
