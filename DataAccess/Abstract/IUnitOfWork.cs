using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IVolunteerDal Volunteers { get; }
        IUserDal Users { get; }
        int Commit();
    }
}
