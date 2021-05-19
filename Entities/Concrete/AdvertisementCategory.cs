using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;
using Entities.Abstract;

namespace Entities.Concrete
{
    public class AdvertisementCategory : BaseModel, IEntity
    {
        public int AdvertisementId { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public Advertisement Advertisement { get; set; }
    }
}
