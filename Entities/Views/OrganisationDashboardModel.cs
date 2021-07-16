using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Views
{
    public class OrganisationDashboardModel
    {
        public int TotalVolunteerCount { get; set; }
        public int TotalActiveProjectCount { get; set; }
        public int TotalVolunteerWorkCount { get; set; }
        public Dictionary<string,int> PurposeCount { get; set; }
        public Dictionary<string,int> CategoryCount { get; set; }
    }
}
