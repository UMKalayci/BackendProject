using Core.DataAccess;
using Entities.Concrete;
using Entities.QueryModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract
{
    public interface ICompanyDal : IEntityRepository<Company>
    {
        Company GetCompanyDashboard(int companyId);
        Company GetCompanyProfilDetail(int companyId);
        ICollection<Volunteer> GetCompanyVolunteerList(int companyId);
        IEnumerable<Company> GetApproveList(AdminCompanyApproveQuery filter = null, PaginationQuery paginationQuery = null);
        int GetApproveCount(AdminCompanyApproveQuery filter = null);
    }
}
