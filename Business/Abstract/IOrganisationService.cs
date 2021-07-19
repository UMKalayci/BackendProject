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
    public interface IOrganisationService
    {
        IDataResult<OrganisationDashboardModel> GetOrganisationDashboard(int organisationId);
        IResult ApproveOrganisation(int organisationId);
        IPaginationResult<List<OrganisationApproveListView>> GetApproveList(AdminOrganisationListQuery organisationListQuery, PaginationQuery paginationQuery = null);
        IDataResult<Organisation> GetOrganisation(int userId);
        IPaginationResult<List<VolunteerListView>> GetOrganisationVolunteerList(int organisationId, PaginationQuery paginationQuery = null);        IResult UserExists(string email);
        IResult ComplatedAdvertisementApprove(VolunteerAdvertisementComplatedApproveDto volunteerAdvertisementComplatedApproveDto);
        IDataResult<List<ApproveListView>> AdvertisementApproveList(int organisationId);
        IDataResult<Organisation> Register(OrganisationForRegisterDto organisationForRegisterDto, string password);
        IDataResult<Organisation> Update(OrganisationForRegisterDto organisationForRegisterDto, string password);
    }
}
