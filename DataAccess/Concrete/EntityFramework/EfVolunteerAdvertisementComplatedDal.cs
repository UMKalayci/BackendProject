using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfVolunteerAdvertisementComplatedDal : EfEntityRepositoryBase<VolunteerAdvertisementComplated, EGonulluContext>, IVolunteerAdvertisementComplatedDal
    {
        public List<VolunteerAdvertisementComplated> GetApproveListByOrganisationId(int organisationId)
        {
            using (var context = new EGonulluContext())
            {
               var query= context.VolunteerAdvertisementComplateds.Where(
                   x=>x.AdvertisementVolunteer.Advertisement.Organisation.OrganisationId == organisationId
                        && x.ConfirmationStatus==2);
                query=query.Include(x => x.AdvertisementVolunteer.Advertisement);
                query = query.Include(x => x.AdvertisementVolunteer.Volunteer.User);

                return query.ToList();
            }
        }
    }
}
