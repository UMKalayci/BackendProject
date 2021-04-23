using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;
using Core.Entities.Concrete;
using Entities.Abstract;

namespace Entities.Concrete
{
    public class Organisation : BaseModel, IEntity
    {
        public int OrganisationId { get; set; }
        public int UserId { get; set; }
        public string OrganisationName { get; set; }
        public DateTime FoundationOfYear{ get; set; }
        public bool IsMemberAcikAcik { get; set; }
        public string Phone { get; set; }
        public byte[] FinanceDocument { get; set; }
        public int CityId { get; set; }
        public byte[] Image { get; set; }
        public string Desc{ get; set; }
        public City City { get; set; }
        public User User { get; set; }
        public ICollection<OrganisationVolunteer> OrganisationVolunteers { get; set; }
        public ICollection<Advertisement> Advertisements { get; set; }
    }
}
