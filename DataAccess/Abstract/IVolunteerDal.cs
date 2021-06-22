using Core.DataAccess;
using Entities.Concrete;
using Entities.QueryModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IVolunteerDal:IEntityRepository<Volunteer>
    {
        IEnumerable<Advertisement> GetAdvertisementList(AdvertisementQuery filter, PaginationQuery paginationQuery = null);
        int GetAdvertisementCount(AdvertisementQuery filter);
    }
}
