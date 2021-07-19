using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Views
{
    public class VolunteerListView
    {
        public int VolunteerId { get; set; }
        public string NameSurname { get; set; }
        public DateTime BirthDate { get; set; }
        public int Gender { get; set; }
        public string CityName { get; set; }
        public string Phone { get; set; }
    }
}
