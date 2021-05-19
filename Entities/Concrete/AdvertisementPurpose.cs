using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;
using Entities.Abstract;

namespace Entities.Concrete
{
    public class AdvertisementPurpose : BaseModel, IEntity
    {
        public int AdvertisementId { get; set; }
        public int PurposeId { get; set; }
        public GlobalPurpose Purpose { get; set; }
        public Advertisement Advertisement { get; set; }
    }
}
