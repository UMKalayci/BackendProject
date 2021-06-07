using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IAdvertisementDal AdvertisementDal { get; }
        IVolunteerDal VolunteerDal { get; }
        IUserDal UserDal { get; }
        ICategoryDal CategoryDal { get; }
        IAdvertisementCategoryDal AdvertisementCategoryDal { get; }
        IAdvertisementPurposeDal AdvertisementPurposeDal { get; }
        IAdvertisementVolunteerDal AdvertisementVolunteerDal { get; }
        int Commit();
    }
}
