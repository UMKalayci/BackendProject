using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Views
{
    public class VolunteerDashboardModel
    {
        public int TotalOrganisationCount { get; set; }
        public int TotalActiveProjectCount { get; set; }
        public int TotalComplatedHours{ get; set; }
        public int TotalComplatedCount { get; set; }
        public Dictionary<string,int> PurposeCount { get; set; }
        public Dictionary<string,int> CategoryCount { get; set; }
    }
}
