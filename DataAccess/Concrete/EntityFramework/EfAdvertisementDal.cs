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
                    var query = context.Advertisements.Where(x => 1 == 1);

                    if(filter.CategoryId!=0)
                    query = query.Where(x => x.AdvertisementCategorys.Any(y=>y.CategoryId==filter.CategoryId));
                    if(filter.PurposeId!=0)
                    query = query.Where(x => x.AdvertisementPurposes.Any(y=>y.PurposeId==filter.PurposeId));


                    query = query.Include(x => x.Organisation);
                    query = query.OrderByDescending(x => x.Status).ThenBy(x => x.StartDate);

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
                    var query = context.Advertisements.Where(x => 1 == 1);

                    query = query.Where(x => x.AdvertisementCategorys.Any(y => y.CategoryId == filter.CategoryId));
                    query = query.Where(x => x.AdvertisementPurposes.Any(y => y.PurposeId == filter.PurposeId));


                    return query.Count();
                }

            }
        }

    }
}
