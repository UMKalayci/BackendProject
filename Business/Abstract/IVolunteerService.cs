using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IVolunteerService
    {
        IDataResult<List<AdvertisementListView>> GetComplatedAdvertisementList(int volunteerId);
        IDataResult<List<AdvertisementListView>> GetActiveAdvertisementList(int volunteerId);
        IDataResult<Volunteer> Update(VolunteerForRegisterDto volunteerForRegisterDto, string password);
        IDataResult<Volunteer> GetVolunteer(int userId);
        IDataResult<VolunteerProfileView> GetVolunterProfile(int userId);
        IDataResult<Volunteer> Register(VolunteerForRegisterDto volunteerForRegisterDto, string password);
        IResult EnrollAdvertisement(AdvertisementVolunteerDto advertisementVolunteerDto);
        IResult ComplatedAdvertisement(VolunteerAdvertisementComplatedDto volunteerAdvertisementComplatedDto);
        IResult UserExists(string email);
    }
}
