using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Views
{
    public class OrganisationProfileView
    {
        public int OrganisationId { get; set; }
        public string NameSurname { get; set; }
        public string OrganisationName { get; set; }
        public DateTime FoundationOfYear { get; set; }
        public bool IsMemberAcikAcik { get; set; }
        public string Phone { get; set; }
        public byte[] FinanceDocument { get; set; }
        public string CityName { get; set; }
        public byte[] Image { get; set; }
        public string Desc { get; set; }
    }
}
