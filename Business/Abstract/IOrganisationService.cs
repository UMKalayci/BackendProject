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
        IResult ApproveOrganisation(int organisationId);
        IPaginationResult<List<OrganisationApproveListView>> GetApproveList(AdminOrganisationListQuery organisationListQuery, PaginationQuery paginationQuery = null);
        IDataResult<Organisation> GetOrganisation(int userId);
        IResult UserExists(string email);
        IResult ComplatedAdvertisementApprove(VolunteerAdvertisementComplatedApproveDto volunteerAdvertisementComplatedApproveDto);
        IDataResult<List<ApproveListView>> AdvertisementApproveList(int organisationId);
        IDataResult<Organisation> Register(OrganisationForRegisterDto organisationForRegisterDto, string password);
    }
}
