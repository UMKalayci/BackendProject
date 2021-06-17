using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserDal:EfEntityRepositoryBase<User, EGonulluContext>,IUserDal
    {
        public List<OperationClaim> GetClaims(User user)
        {
            using (var context = new EGonulluContext())
            {
                var result = context.UserOperationClaims.Where(x => x.UserId == user.Id)
                    .Include(x=>x.OperationClaim)
                    .Select(x => new OperationClaim()
                    {
                        Id = x.OperationClaimId,
                        Name = x.OperationClaim.Name
                    });

                return result.ToList();

            }
        }
    }
}
