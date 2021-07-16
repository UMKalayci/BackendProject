using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Core.Entities;
using Entities.Abstract;

namespace Entities.Concrete
{
    public class AdvertisementVolunteer : BaseModel, IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int AdvertisementId { get; set; }
        public int VolunteerId { get; set; }
        public Volunteer Volunteer { get; set; }
        public Advertisement Advertisement { get; set; }
        public ICollection<VolunteerAdvertisementComplated> VolunteerAdvertisementComplateds { get; set; }

    }
}
