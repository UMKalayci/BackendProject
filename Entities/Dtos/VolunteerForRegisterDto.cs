using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;

namespace Entities.Dtos
{
    public class VolunteerForRegisterDto : IDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int Gender { get; set; }
        public int? UniversityId { get; set; }
        public string HighSchool { get; set; }
        public int? CompanyId { get; set; }
        public int? CompanyDepartmentId { get; set; }
        public int CityId { get; set; }
        public string Phone { get; set; }

    }
}
