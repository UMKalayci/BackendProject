using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;

namespace Entities.Dtos
{
    public class OrganisationForRegisterDto : IDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OrganisationName { get; set; }
        public DateTime FoundationOfYear { get; set; }
        public bool IsMemberAcikAcik { get; set; }
        public string Phone { get; set; }
        public byte[] FinanceDocument { get; set; }
        public int CityId { get; set; }
        public byte[] Image { get; set; }
        public string Desc { get; set; }

    }
}
