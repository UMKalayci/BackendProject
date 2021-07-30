using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using Entities.QueryModels;
using Entities.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface ICompanyService
    {
        IPaginationResult<List<CompanyView>> GetApproveList(AdminCompanyApproveQuery companyQuery, PaginationQuery paginationQuery);
        IDataResult<List<VolunteerListView>> GetCompanyVolunteerList(int companyId);
        IDataResult<Company> Register(CompanyForRegisterDto companyForRegisterDto, string password);
        IResult UserExists(string email);
        IDataResult<Company> GetCompany(int userId);
        IDataResult<CompanyView> GetProfilDetail(int companyId);
        IDataResult<List<CompanyComboListView>> GetComboList();
        IDataResult<CompanyDashboardView> GetCompanyDashboard(int companyId);
        IResult ApproveCompany(int companyId);
    }
}
