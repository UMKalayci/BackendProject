using Core.Entities;
using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class Comment : BaseModel, IEntity
    {
        public int Id { get; set; }
        public int AdvertisementId { get; set; }
        public int VolunteerId { get; set; }
        public string Desc { get; set; }
        public Advertisement Advertisement { get; set; }
        public Volunteer Volunteer { get; set; }
    }
}
