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
    public class EfOrganisationDal : EfEntityRepositoryBase<Organisation, EGonulluContext>, IOrganisationDal
    {
        public Organisation GetOrganisationDashboard(int organisationId)
        {
            using (var context = new EGonulluContext())
            {
                var query = context.Organisations.Where(x => x.OrganisationId == organisationId);
                query = query.Where(x => x.Advertisements.Any(y => y.Status == true && y.EndDate>DateTime.Now));
                query = query.Include(x => x.Advertisements);
                query = query.Include(x => x.Advertisements)
                    .ThenInclude(x => x.AdvertisementVolunteers);
                query = query.Include(x => x.Advertisements)
                    .ThenInclude(x => x.AdvertisementPurposes);
                query = query.Include(x => x.Advertisements)
                    .ThenInclude(x => x.AdvertisementPurposes)
                    .ThenInclude(x=>x.Purpose);
                query = query.Include(x => x.Advertisements)
                    .ThenInclude(x => x.AdvertisementCategorys);
                query = query.Include(x => x.Advertisements)
                    .ThenInclude(x => x.AdvertisementCategorys)
                    .ThenInclude(x=>x.Category);
                query = query.Include(x => x.Advertisements)
                    .ThenInclude(x => x.AdvertisementVolunteers);
                query = query.Include(x => x.Advertisements)
                    .ThenInclude(x => x.AdvertisementVolunteers)
                    .ThenInclude(x => x.VolunteerAdvertisementComplateds);
                return query.FirstOrDefault();
            }
        }
        public IEnumerable<Organisation> GetApproveList(AdminOrganisationListQuery filter = null, PaginationQuery paginationQuery = null)
        {
            using (var context = new EGonulluContext())
            {

                if (filter == null)
                {
                    if (paginationQuery != null)
                        return context.Set<Organisation>().Skip((paginationQuery.PageNumber - 1) * paginationQuery.PageSize).Take(paginationQuery.PageSize).ToList();
                    else
                        return context.Set<Organisation>().ToList();
                }
                else
                {
                    var query = context.Organisations.Where(x => 1 == 1);

                    if (filter.OrganisationName != null)
                        query = query.Where(x => x.OrganisationName == filter.OrganisationName);


                    query = query.Include(x => x.City);
                    query = query.OrderByDescending(x => x.Status).ThenBy(x => x.InsertDate);
                    if (paginationQuery != null)
                        query = query.Skip((paginationQuery.PageNumber - 1) * paginationQuery.PageSize).Take(paginationQuery.PageSize);
                    return query.ToList();
                }


            }
        }


        public int GetApproveCount(AdminOrganisationListQuery filter = null)
        {
            using (var context = new EGonulluContext())
            {
                if (filter == null)
                {
                    return context.Set<Organisation>().Count();
                }
                else
                {
                    var query = context.Organisations.Where(x => 1 == 1);

                    if (filter.OrganisationName != null)
                        query = query.Where(x => x.OrganisationName == filter.OrganisationName);

                    return query.Count();
                }

            }
        }
    }
}
