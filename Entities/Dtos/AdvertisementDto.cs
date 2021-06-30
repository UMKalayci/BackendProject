using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class AdvertisementDto
    {
        public int AdvertisementId { get; set; }
        public string AdvertisementTitle { get; set; }
        public string AdvertisementDesc { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime AppStartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime AppEndDate { get; set; }
        public int OrganisationId { get; set; }
        public bool IsOnline { get; set; }
        public List<int> CategoryIdList { get; set; }
        public List<int>  PurposeIdList{ get; set; }
        public int CityId { get; set; }
        public byte[] Image { get; set; }
        public bool IsApplied { get; set; }
    }
}
