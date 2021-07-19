using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Views
{
    public class AdvertisementDetailView
    {
        public string Name { get; set; }
        public string Corporation { get; set; }
        public int ApplicantCount { get; set; }
        public bool  IsApplied { get; set; }
        public string Description { get; set; }
        public byte[] ProjectImage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ApplicationStartDate { get; set; }
        public DateTime ApplicationEndDate { get; set; }
        public string   Location { get; set; }
        public List<String> Categories { get; set; }
        public List<String> Purposes { get; set; }
        public List<CommentView> CommentList { get; set; }
    }
}
