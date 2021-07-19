using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Views
{
    public class CommentView
    {
        public int Id { get; set; }
        public string VolunteerName { get; set; }
        public DateTime InsertDate { get; set; }
        public string Desc { get; set; }
    }
}
