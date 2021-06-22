using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Core.Entities;
using Entities.Abstract;

namespace Entities.Concrete
{
    public class VolunteerAdvertisementComplated : BaseModel, IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int AdvertisementVolunteerId { get; set; }
        public int TotalWork { get; set; }
        public int ConfirmationStatus { get; set; }//0 red 1 onaylandı 2 onay bekliyor
        public AdvertisementVolunteer AdvertisementVolunteer { get; set; }
    }
}
