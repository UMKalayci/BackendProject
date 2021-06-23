using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class VolunteerAdvertisementComplatedApproveDto
    {
        public int OrganisationId { get; set; }
        public int ComplatedId { get; set; }
        public int Approve { get; set; }
    }
}
