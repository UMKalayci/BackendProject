using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;
using Entities.Abstract;

namespace Entities.Concrete
{
    public class OrganisationVolunteer : BaseModel, IEntity
    {
        public int OrganisationId { get; set; }
        public int VolunteerId { get; set; }
        public Volunteer Volunteer { get; set; }
        public Organisation Organisation { get; set; }
    }
}
