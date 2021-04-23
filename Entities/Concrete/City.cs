using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;
using Entities.Abstract;

namespace Entities.Concrete
{
    public class City : BaseModel, IEntity
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
    }
}
