using System;
using System.Collections.Generic;
using Core.Entities;
using Core.Entities.Concrete;
using Entities.Abstract;

namespace Entities.Concrete
{
    public class Volunteer: BaseModel, IEntity
    {
        public int VolunteerId { get; set; }
        public int UserId { get; set; }
        public DateTime BirthDate { get; set; }
        public int Gender { get; set; }
        public int? UniversityId { get; set; }
        public string HighSchool { get; set; }
        public int? CompanyId { get; set; }
        public int? CompanyDepartmentId { get; set; }
        public int CityId { get; set; }
        public string Phone { get; set; }

        public User User { get; set; }
        public University University { get; set; }
        public Company Company { get; set; }
        public City City { get; set; }
        public CompanyDepartment CompanyDepartment { get; set; }
        public ICollection<OrganisationVolunteer> OrganisationVolunteer { get; set; }
        public ICollection<AdvertisementVolunteer> AdvertisementVolunteers { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
