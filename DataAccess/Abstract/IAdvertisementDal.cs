using Core.DataAccess;
using Entities.Concrete;
using Entities.QueryModels;
using Entities.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IAdvertisementDal : IEntityRepository<Advertisement>
    {
        IEnumerable<Advertisement> GetList(AdvertisementQuery filter, PaginationQuery paginationQuery = null);
        IEnumerable<Advertisement> GetApproveList(AdminAdvertisementApproveQuery filter, PaginationQuery paginationQuery = null);
        Advertisement GetDetail(int advertisementId);
        int GetCount(AdvertisementQuery filter);
        int GetApproveCount(AdminAdvertisementApproveQuery filter);
    }
}
