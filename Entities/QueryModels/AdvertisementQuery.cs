using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.QueryModels
{
    public class AdvertisementQuery
    {
        public int VolunteerId { get; set; }
        public int CategoryId { get; set; }
        public int PurposeId { get; set; }
    }
}
