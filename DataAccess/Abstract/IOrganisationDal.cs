using Core.DataAccess;
using Entities.Concrete;
using Entities.QueryModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IOrganisationDal : IEntityRepository<Organisation>
    {
        IEnumerable<Organisation> GetApproveList(AdminOrganisationListQuery filter = null, PaginationQuery paginationQuery = null);
        int GetApproveCount(AdminOrganisationListQuery filter = null);
    }
}
