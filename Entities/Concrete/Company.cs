using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;
using Core.Entities.Concrete;
using Entities.Abstract;

namespace Entities.Concrete
{
    public class Company : BaseModel, IEntity
    {
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public string CompanyName { get; set; }
        public DateTime FoundationOfYear { get; set; }
        public string Phone { get; set; }
        public int CityId { get; set; }
        public byte[] Image { get; set; }
        public string Desc { get; set; }
        public ICollection<CompanyDepartment> CompanyDepartments { get; set; }
        public User User { get; set; }
    }
}
