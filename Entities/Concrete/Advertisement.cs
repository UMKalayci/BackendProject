using Core.Entities;
using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class Advertisement :BaseModel, IEntity
    {
        public int AdvertisementId { get; set; }
        public string AdvertisementTitle { get; set; }
        public string AdvertisementDesc { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime AppStartDate { get; set; }
        public DateTime AppEndDate { get; set; }
        public int OrganisationId { get; set; }
        public bool IsOnline { get; set; }

        public ICollection<AdvertisementCategory> AdvertisementCategorys { get; set; }
        public ICollection<AdvertisementPurpose> AdvertisementPurposes { get; set; }
        public ICollection<AdvertisementVolunteer> AdvertisementVolunteers { get; set; }
        public Organisation Organisation { get; set; }
    }
}
