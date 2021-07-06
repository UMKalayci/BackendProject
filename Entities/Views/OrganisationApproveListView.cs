using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Views
{
    public class OrganisationApproveListView
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public DateTime FoundationOfYear { get; set; }
        public bool IsMemberAcikAcik { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Desc { get; set; }
        public bool Status { get; set; }
    }
}
