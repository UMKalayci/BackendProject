using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;
using Entities.Abstract;

namespace Entities.Concrete
{
    public class AdvertisementVolunteer : BaseModel, IEntity
    {
        public int AdvertisementId { get; set; }
        public int VolunteerId { get; set; }
        public Volunteer Volunteer { get; set; }
        public Advertisement Advertisement { get; set; }
    }
}
