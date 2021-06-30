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
    public class EfVolunteerDal : EfEntityRepositoryBase<Volunteer, EGonulluContext>, IVolunteerDal
    {
        public IEnumerable<Advertisement> GetAdvertisementList(AdvertisementQuery filter = null, PaginationQuery paginationQuery = null)
        {
            using (var context = new EGonulluContext())
            {

                var query = context.AdvertisementVolunteers
                                    .Where(x => x.VolunteerId == filter.VolunteerId)
                                    .Select(x => x.Advertisement);

                if (filter == null)
                {
                    if (paginationQuery != null)
                        return query.Skip((paginationQuery.PageNumber - 1) * paginationQuery.PageSize).Take(paginationQuery.PageSize).ToList();
                    else
                        return query.ToList();
                }
                else
                {

                    if (filter.CategoryId != 0)
                        query = query.Where(x => x.AdvertisementCategorys.Any(y => y.CategoryId == filter.CategoryId));
                    if (filter.PurposeId != 0)
                        query = query.Where(x => x.AdvertisementPurposes.Any(y => y.PurposeId == filter.PurposeId));


                    query = query.OrderByDescending(x => x.Status).ThenBy(x => x.StartDate);
                    if (paginationQuery != null)
                        query = query.Skip((paginationQuery.PageNumber - 1) * paginationQuery.PageSize).Take(paginationQuery.PageSize);
                    return query.ToList();
                }


            }
        }

        public Volunteer GetVolunterProfile(int userId)
        {
            using (var context = new EGonulluContext())
            {
               var query= context.Volunteers.Where(x => x.UserId == userId)
                    .Include(x => x.User).Include(x => x.City);

                return query.FirstOrDefault();
            }

        }
        public int GetAdvertisementCount(AdvertisementQuery filter = null)
        {
            using (var context = new EGonulluContext())
            {
                var query = context.AdvertisementVolunteers
                                    .Where(x => x.VolunteerId == filter.VolunteerId)
                                    .Select(x => x.Advertisement);
                if (filter == null)
                {
                    return query.Count();
                }
                else
                {
                    query = query.Where(x => x.AdvertisementCategorys.Any(y => y.CategoryId == filter.CategoryId));
                    query = query.Where(x => x.AdvertisementPurposes.Any(y => y.PurposeId == filter.PurposeId));


                    return query.Count();
                }

            }
        }

    }
}
