using Core.Entities;
using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class VolunteerDoneAdvertisement : BaseModel, IEntity
    {
        public int  VolunteerDoneAdvertisementId { get; set; }
        public int AdvertisementId { get; set; }
        public int VolunteerId { get; set; }
        public decimal WorkTime { get; set; }
        public DateTime ComplatedTime { get; set; }
        public int Statu { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public Volunteer Volunteer { get; set; }
        public Advertisement Advertisement { get; set; }
    }
}
