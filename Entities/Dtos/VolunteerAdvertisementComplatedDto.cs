using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class VolunteerAdvertisementComplatedDto
    {
        public int VolunteerId { get; set; }
        public int AdvertisementId { get; set; }
        public int TotalWork { get; set; }
    }
}
