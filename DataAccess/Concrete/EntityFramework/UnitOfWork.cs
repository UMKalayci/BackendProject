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

        public UnitOfWork(EGonulluContext context)
        {
            this._context = context;
        }

        public IUserDal Users => _userDal = _userDal ?? new EfUserDal(_context);

        public IVolunteerDal Volunteers => _vulunteerDal = _vulunteerDal ?? new EfVolunteerDal(_context);
        public IAdvertisementDal Advertisements => _advertisementDal = _advertisementDal ?? new EfAdvertisementDal(_context);

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
