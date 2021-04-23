using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;
using Core.Entities.Concrete;
using Entities.Abstract;

namespace Entities.Concrete
{
    public class University : BaseModel, IEntity
    {
        public int UniversityId { get; set; }
        public string UniversityName { get; set; }
        public DateTime FoundationOfYear { get; set; }
        public string Phone { get; set; }
        public int CityId { get; set; }
        public byte[] Image { get; set; }
        public string Desc { get; set; }
        public User User { get; set; }
    }
}
