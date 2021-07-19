using Core.Entities;
using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DefaultValue(1)]
        public int CityId { get; set; }
        public byte[] Image { get; set; }
        public bool IsApplied { get; set; }
        public ICollection<AdvertisementCategory> AdvertisementCategorys { get; set; }
        public ICollection<AdvertisementPurpose> AdvertisementPurposes { get; set; }
        public ICollection<AdvertisementVolunteer> AdvertisementVolunteers { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public Organisation Organisation { get; set; }
        public City City { get; set; }
    }
}
