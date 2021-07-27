using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCompanyDal : EfEntityRepositoryBase<Company, EGonulluContext>, ICompanyDal
    {
        public ICollection<Volunteer> GetCompanyVolunteerList(int companyId)
        {
            using (var context = new EGonulluContext())
            {
                var volunteerList = context.Companys
                                    .Where(x => x.CompanyId == companyId)
                                    .Include(x => x.Volunteers)
                                    .Include(x=>x.Volunteers).ThenInclude(x=>x.User)
                                    .Include(x=>x.Volunteers).ThenInclude(x=>x.City)
                                    .FirstOrDefault().Volunteers;

                return volunteerList;
            }
        }
    }
}
