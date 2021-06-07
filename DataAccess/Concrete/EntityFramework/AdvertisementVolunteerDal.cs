using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework
{
    public class AdvertisementVolunteerDal : EfEntityRepositoryBase<AdvertisementVolunteer>, IAdvertisementVolunteerDal
    {
        public AdvertisementVolunteerDal(EGonulluContext context)
         : base(context)
        { }

    }
}
