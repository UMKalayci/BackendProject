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
        public Volunteer GetVolunteerDashboard(int volunteerId)
        {
            using (var context = new EGonulluContext())
            {
                var query = context.Volunteers.Where(x => x.VolunteerId == volunteerId);
                query = query.Include(x => x.AdvertisementVolunteers);
                query = query
                    .Include(x => x.AdvertisementVolunteers)
                    .ThenInclude(x => x.Advertisement);

                query = query
                    .Include(x => x.AdvertisementVolunteers)
                    .ThenInclude(x => x.Advertisement)
                    .ThenInclude(x => x.AdvertisementPurposes);

                query = query
                    .Include(x => x.AdvertisementVolunteers)
                    .ThenInclude(x => x.Advertisement)
                    .ThenInclude(x => x.AdvertisementCategorys);


                query = query
                    .Include(x => x.AdvertisementVolunteers)
                    .ThenInclude(x => x.Advertisement)
                    .ThenInclude(x => x.AdvertisementPurposes)
                    .ThenInclude(x=>x.Purpose);

                query = query
                    .Include(x => x.AdvertisementVolunteers)
                    .ThenInclude(x => x.Advertisement)
                    .ThenInclude(x => x.AdvertisementCategorys)
                    .ThenInclude(x=>x.Category);


                query = query
                    .Include(x => x.AdvertisementVolunteers)
                    .ThenInclude(x => x.Advertisement)
                    .ThenInclude(x => x.Organisation);
                query = query.Include(x => x.AdvertisementVolunteers)
                    .ThenInclude(x => x.VolunteerAdvertisementComplateds);

                return query.FirstOrDefault();
            }
        }
        public IEnumerable<Advertisement> GetAdvertisementList(AdvertisementQuery filter = null, PaginationQuery paginationQuery = null)
        {
            using (var context = new EGonulluContext())
            {

                var query = context.AdvertisementVolunteers
                                    .Where(x => x.VolunteerId == filter.VolunteerId)
                                    .Select(x => x.Advertisement);
                query = query.Include(x => x.Organisation);
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

                    if (filter.Complated == true)
                    {
                        query = query.Where(x => x.AdvertisementVolunteers
                                     .Any(y => y.VolunteerAdvertisementComplateds
                                     .Any(z => z.ConfirmationStatus == 1)));
                    }
                    else if (filter.Complated == false)
                    {
                        query = query.Where(x => x.AdvertisementVolunteers
                                     .Any(y => y.VolunteerAdvertisementComplateds
                                     .Any(z => z.ConfirmationStatus == 1)) == false);
                    }
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
                var query = context.Volunteers.Where(x => x.UserId == userId)
                     .Include(x => x.User).Include(x => x.City);

                return query.FirstOrDefault();
            }

        }

        public bool IsAdvertisementEnroll(int advertisementId)
        {
            using (var context = new EGonulluContext())
            {
                return context.Volunteers.Any(x => x.AdvertisementVolunteers.Any(y => y.AdvertisementId == advertisementId));
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

        public List<Volunteer> GetVolunteersList()
        {
            using (var context = new EGonulluContext())
            {
                var query = context.Volunteers.Where(x=>1==1);
                query = query.Include(x => x.User);
                return query.ToList();
            }
        }


    }
}
