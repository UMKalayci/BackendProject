using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IVolunteerService
    {
        IDataResult<Volunteer> Register(VolunteerForRegisterDto volunteerForRegisterDto, string password);

        IResult UserExists(string email);
    }
}
