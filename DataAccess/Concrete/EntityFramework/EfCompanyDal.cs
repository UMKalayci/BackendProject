using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.QueryModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCompanyDal : EfEntityRepositoryBase<Company, EGonulluContext>, ICompanyDal
    {
        public Company GetCompanyDashboard(int companyId)
        {
            using (var context = new EGonulluContext())
            {
                var query = context.Companys.Where(x => x.CompanyId == companyId);
                query = query.Include(x => x.Volunteers);
                query = query.Include(x => x.Volunteers)
                    .ThenInclude(x => x.AdvertisementVolunteers);
                query = query.Include(x => x.Volunteers)
                    .ThenInclude(x => x.AdvertisementVolunteers)
                    .ThenInclude(x=>x.VolunteerAdvertisementComplateds);
                return query.FirstOrDefault();
            }
        }
        public ICollection<Volunteer> GetCompanyVolunteerList(int companyId)
        {
            using (var context = new EGonulluContext())
            {
                var volunteerList = context.Companys
                                    .Where(x => x.CompanyId == companyId)
                                    .Include(x => x.Volunteers)
                                    .Include(x => x.Volunteers).ThenInclude(x => x.User)
                                    .Include(x => x.Volunteers).ThenInclude(x => x.City)
                                    .FirstOrDefault().Volunteers;

                return volunteerList;
            }
        }

        public Company GetCompanyProfilDetail(int companyId)
        {
            using (var context = new EGonulluContext())
            {
                var query = context.Companys.Where(x => x.CompanyId == companyId)
                                                 .Include(x => x.City)
                                                 .Include(x => x.User);
                return query.FirstOrDefault();
            }
        }


        public IEnumerable<Company> GetApproveList(AdminCompanyApproveQuery filter = null, PaginationQuery paginationQuery = null)
        {
            using (var context = new EGonulluContext())
            {

                if (filter == null)
                {
                    if (paginationQuery != null)
                        return context.Set<Company>().Skip((paginationQuery.PageNumber - 1) * paginationQuery.PageSize).Take(paginationQuery.PageSize).ToList();
                    else
                        return context.Set<Company>().ToList();
                }
                else
                {
                    var query = context.Companys.Where(x => 1 == 1);

                    if (filter.CompanyName != null)
                        query = query.Where(x => x.CompanyName == filter.CompanyName);


                    query = query.Include(x => x.City);
                    query = query.OrderBy(x => x.Status).ThenBy(x => x.InsertDate);
                    if (paginationQuery != null)
                        query = query.Skip((paginationQuery.PageNumber - 1) * paginationQuery.PageSize).Take(paginationQuery.PageSize);
                    return query.ToList();
                }


            }
        }

        public int GetApproveCount(AdminCompanyApproveQuery filter = null)
        {
            using (var context = new EGonulluContext())
            {
                if (filter == null)
                {
                    return context.Set<Company>().Count();
                }
                else
                {
                    var query = context.Companys.Where(x => 1 == 1);

                    if (filter.CompanyName != null)
                        query = query.Where(x => x.CompanyName == filter.CompanyName);

                    return query.Count();
                }

            }
        }

    }
}
