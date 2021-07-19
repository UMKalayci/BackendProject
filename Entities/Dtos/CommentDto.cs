using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }
        public int AdvertisementId { get; set; }
        public int VolunteerId { get; set; }
        public string Desc { get; set; }
    }
}
