using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EGonulluContext _context;
        private EfUserDal _userDal;
        private EfVolunteerDal _vulunteerDal;
        private EfAdvertisementDal _advertisementDal;
        private EfCategoryDal _categoryDal;
        private AdvertisementCategoryDal _advertisementCategoryDal;
        private AdvertisementPurposeDal _advertisementPurposeDal;
        private AdvertisementVolunteerDal _advertisementVolunteerDal;

        public UnitOfWork(EGonulluContext context)
        {
            this._context = context;
        }

        public IUserDal UserDal => _userDal = _userDal ?? new EfUserDal(_context);

        public IVolunteerDal VolunteerDal => _vulunteerDal = _vulunteerDal ?? new EfVolunteerDal(_context);
        public IAdvertisementDal AdvertisementDal => _advertisementDal = _advertisementDal ?? new EfAdvertisementDal(_context);
        public ICategoryDal CategoryDal => _categoryDal = _categoryDal ?? new EfCategoryDal(_context);
        public IAdvertisementCategoryDal AdvertisementCategoryDal => _advertisementCategoryDal = _advertisementCategoryDal ?? new AdvertisementCategoryDal(_context);
        public IAdvertisementPurposeDal AdvertisementPurposeDal => _advertisementPurposeDal = _advertisementPurposeDal ?? new AdvertisementPurposeDal(_context);
        public IAdvertisementVolunteerDal AdvertisementVolunteerDal => _advertisementVolunteerDal = _advertisementVolunteerDal ?? new AdvertisementVolunteerDal(_context);

        public int Commit()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch(Exception ex)
            {
                Dispose();
                return 0;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
