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
        IEnumerable<Organisation> GetApproveList(OrganisationListQuery filter = null, PaginationQuery paginationQuery = null);
        int GetApproveCount(OrganisationListQuery filter = null);
    }
}
