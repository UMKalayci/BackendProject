using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Views
{
    public class ApproveListView
    {
        public int ComplatedId { get; set; }
        public string AdvertisementName { get; set; }
        public string VolunteerName { get; set; }
        public int TotalWork { get; set; }
        public int ConfirmationStatus { get; set; }//0 red 1 onaylandı 2 onay bekliyor
    }
}
