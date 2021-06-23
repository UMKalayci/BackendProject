using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Views
{
    public class ApproveListView
    {
        public Advertisement Advertisement { get; set; }
        public Volunteer Volunteer { get; set; }
        public int TotalWork { get; set; }
        public int ConfirmationStatus { get; set; }//0 red 1 onaylandı 2 onay bekliyor
    }
}
