using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Views
{
    public class CompanyView
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public DateTime FoundationOfYear { get; set; }
        public string Phone { get; set; }
        public string CityName { get; set; }
        public byte[] Image { get; set; }
        public string Desc { get; set; }
        public bool Status { get; set; }
    }
}
