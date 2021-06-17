using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IOrganisationService
    {
        IDataResult<Organisation> GetOrganisation(int userId);
        IResult UserExists(string email);
        IDataResult<Organisation> Register(OrganisationForRegisterDto organisationForRegisterDto, string password);
    }
}
