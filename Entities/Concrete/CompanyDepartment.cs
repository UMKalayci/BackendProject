using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;
using Entities.Abstract;

namespace Entities.Concrete
{
    public class CompanyDepartment : BaseModel, IEntity
    {
        public int CompanyDepartmentId { get; set; }
        public string CompanyDepartmentName { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
