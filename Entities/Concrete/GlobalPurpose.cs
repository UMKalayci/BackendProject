using Core.Entities;
using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class GlobalPurpose : BaseModel, IEntity
    {
        public int PurposeId { get; set; }
        public string PurposeName { get; set; }

        public ICollection<AdvertisementPurpose> AdvertisementPurposes { get; set; }
    }
}
