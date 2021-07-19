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
    public class EfAdvertisementDal : EfEntityRepositoryBase<Advertisement, EGonulluContext>, IAdvertisementDal
    {
        public Advertisement GetDetail(int advertisementId)
        {
            using (var context = new EGonulluContext())
            {
              var advertisement= context.Advertisements.Where(x => x.AdvertisementId == advertisementId)
                                        .Include(x=>x.AdvertisementCategorys).ThenInclude(x=>x.Category)
                                        .Include(x=>x.AdvertisementPurposes).ThenInclude(x=>x.Purpose)
                                        .Include(x=>x.City)
                                        .Include(x=>x.Organisation)
                                        .Include(x=>x.Comments)
                                        .Include(x=>x.Comments).ThenInclude(x=>x.Volunteer).ThenInclude(x=>x.User)
                                        .Include(x=>x.AdvertisementVolunteers).FirstOrDefault();

                return advertisement;
            }
        }
        public IEnumerable<Advertisement> GetList(AdvertisementQuery filter = null, PaginationQuery paginationQuery = null)
        {
            using (var context = new EGonulluContext())
            {

                if (filter == null)
                {
                    if (paginationQuery != null)
                        return context.Set<Advertisement>().Skip((paginationQuery.PageNumber - 1) * paginationQuery.PageSize).Take(paginationQuery.PageSize).ToList();
                    else
                        return context.Set<Advertisement>().ToList();
                }
                else
                {
                    var query = context.Advertisements.Where(x => x.Status == true);

                    if (filter.CategoryId != 0)
                        query = query.Where(x => x.AdvertisementCategorys.Any(y => y.CategoryId == filter.CategoryId));
                    if (filter.PurposeId != 0)
                        query = query.Where(x => x.AdvertisementPurposes.Any(y => y.PurposeId == filter.PurposeId));
                    if (filter.OrganisationId != 0)
                        query = query.Where(x => x.OrganisationId==filter.OrganisationId);

                    if (filter.Complated ==true)
                        query = query.Where(x => x.EndDate<DateTime.Now);
                    if (filter.Complated == false)
                        query = query.Where(x => x.EndDate > DateTime.Now);


                    query = query.Include(x => x.Organisation);
                    query = query.OrderByDescending(x => x.Status).ThenBy(x => x.StartDate);
                    if (paginationQuery != null)
                        query = query.Skip((paginationQuery.PageNumber - 1) * paginationQuery.PageSize).Take(paginationQuery.PageSize);
                    return query.ToList();
                }


            }
        }
        public IEnumerable<Advertisement> GetApproveList(AdminAdvertisementApproveQuery filter = null, PaginationQuery paginationQuery = null)
        {
            using (var context = new EGonulluContext())
            {

                if (filter == null)
                {
                    if (paginationQuery != null)
                        return context.Set<Advertisement>().Skip((paginationQuery.PageNumber - 1) * paginationQuery.PageSize).Take(paginationQuery.PageSize).ToList();
                    else
                        return context.Set<Advertisement>().ToList();
                }
                else
                {
                    var query = context.Advertisements.Where(x => x.Status == false);

                    if (filter.OrganisationName != null)
                        query = query.Where(x => x.Organisation.OrganisationName==filter.OrganisationName);


                    query = query.Include(x => x.Organisation);
                    query = query.OrderByDescending(x => x.Status).ThenBy(x => x.StartDate);
                    if (paginationQuery != null)
                        query = query.Skip((paginationQuery.PageNumber - 1) * paginationQuery.PageSize).Take(paginationQuery.PageSize);
                    return query.ToList();
                }


            }
        }


        public int GetCount(AdvertisementQuery filter = null)
        {
            using (var context = new EGonulluContext())
            {
                if (filter == null)
                {
                    return context.Set<Advertisement>().Count();
                }
                else
                {
                    var query = context.Advertisements.Where(x => x.Status == true);
                    if (filter.CategoryId != 0)
                        query = query.Where(x => x.AdvertisementCategorys.Any(y => y.CategoryId == filter.CategoryId));
                    if (filter.PurposeId != 0)
                        query = query.Where(x => x.AdvertisementPurposes.Any(y => y.PurposeId == filter.PurposeId));
                    if (filter.OrganisationId != 0)
                        query = query.Where(x => x.OrganisationId == filter.OrganisationId);

                    return query.Count();
                }

            }
        }
        public int GetApproveCount(AdminAdvertisementApproveQuery filter = null)
        {
            using (var context = new EGonulluContext())
            {
                if (filter == null)
                {
                    return context.Set<Advertisement>().Count();
                }
                else
                {
                    var query = context.Advertisements.Where(x => x.Status == false);

                    if (filter.OrganisationName != null)
                        query = query.Where(x => x.Organisation.OrganisationName == filter.OrganisationName);

                    return query.Count();
                }

            }
        }

    }
}
