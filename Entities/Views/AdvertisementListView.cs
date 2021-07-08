using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Views
{
    public class AdvertisementListView
    {
        public int AdvertisementId { get; set; }
        public string AdvertisementTitle { get; set; }
        public string AdvertisementDesc { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public bool IsOnline { get; set; }
        public bool Record{ get; set; }
        public bool Status{ get; set; }
    }
}
