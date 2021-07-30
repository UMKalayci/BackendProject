using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Views
{
    public class CompanyDashboardView
    {
        public int VolunteerCount { get; set; }
        public int VolunteerProjectCount { get; set; }
        public int VolunteerComplatedCount { get; set; }
        public int VolunteerTotalWorkHours { get; set; }
    }
}
