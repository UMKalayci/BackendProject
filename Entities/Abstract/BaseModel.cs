using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Abstract
{
    public abstract class BaseModel
    {
        public DateTime InsertDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool Status { get; set; }
    }
}
