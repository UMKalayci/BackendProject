using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.CrossCuttingConcerns.Validation;
using Core.Entities.Concrete;
using Core.Extensions;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.Dtos;
using Entities.QueryModels;
using Entities.Views;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Business.Concrete
{
    public class VolunteerManager : IVolunteerService
    {
        private IUserService _userService;
        private IAdvertisementVolunteerDal _advertisementVolunteerDal;
        private IVolunteerAdvertisementComplatedDal _volunteerAdvertisementComplatedDal;
        private IVolunteerDal _volunteerDal;
        private IAdvertisementDal _advertisementDal;

        public VolunteerManager(IUserService userService, IAdvertisementDal advertisementDal, IVolunteerAdvertisementComplatedDal volunteerAdvertisementComplatedDal, IAdvertisementVolunteerDal advertisementVolunteerDal, IVolunteerDal volunteerDal)
        {
            _advertisementVolunteerDal = advertisementVolunteerDal;
            _userService = userService;
            _volunteerDal = volunteerDal;
            _volunteerAdvertisementComplatedDal = volunteerAdvertisementComplatedDal;
            _advertisementDal = advertisementDal;
        }
        public IDataResult<Volunteer> GetVolunteer(int userId)
        {
            var volunteer = _volunteerDal.Get(x => x.UserId == userId);
            if (volunteer != null)
                return new SuccessDataResult<Volunteer>(volunteer);
            else
                return new ErrorDataResult<Volunteer>(null, Messages.UserNotFound);
        }

        public IDataResult<VolunteerProfileView> GetVolunterProfile(int userId)
        {
            var volunteer = _volunteerDal.GetVolunterProfile(userId);
            VolunteerProfileView profileView = new VolunteerProfileView();
            profileView.Location = volunteer.City.CityName;
            profileView.Name = volunteer.User.FirstName;
            profileView.Surname = volunteer.User.LastName;
            profileView.VolunteerSince = volunteer.InsertDate;

            return new SuccessDataResult<VolunteerProfileView>(profileView);
        }
        [ValidationAspect(typeof(VolunteerValidator), Priority = 1)]
        public IDataResult<Volunteer> Register(VolunteerForRegisterDto volunteerForRegisterDto, string password)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var user = new User
            {
                Email = volunteerForRegisterDto.Email,
                FirstName = volunteerForRegisterDto.FirstName,
                LastName = volunteerForRegisterDto.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true
            };
            try
            {
                _userService.Add(user);
                int userId = _userService.GetByMail(user.Email).Id;

                UserOperationClaim userOperationClaim = new UserOperationClaim();
                userOperationClaim.UserId = userId;
                userOperationClaim.OperationClaimId = 1;
                _userService.AddUserClaims(userOperationClaim);

                var volunter = new Volunteer
                {
                    UserId = userId,
                    CityId = volunteerForRegisterDto.CityId,
                    BirthDate = volunteerForRegisterDto.BirthDate,
                    CompanyId = volunteerForRegisterDto.CompanyId,
                    Gender = volunteerForRegisterDto.Gender,
                    Phone = volunteerForRegisterDto.Phone,
                    UniversityId = volunteerForRegisterDto.UniversityId,
                    CompanyDepartmentId = volunteerForRegisterDto.CompanyDepartmentId,
                    HighSchool = volunteerForRegisterDto.HighSchool,
                    UpdateDate = DateTime.Now,
                    InsertDate = DateTime.Now,
                    Status = true
                };

                _volunteerDal.Add(volunter);
                volunter.User = user;
                return new SuccessDataResult<Volunteer>(volunter, Messages.UserRegistered);
            }
            catch (Exception hata)
            {
                return new ErrorDataResult<Volunteer>(Messages.VolunteerAddError);
            }
        }

        [ValidationAspect(typeof(VolunteerValidator), Priority = 1)]
        public IDataResult<Volunteer> Update(VolunteerForRegisterDto volunteerForRegisterDto, string password)
        {
            try
            {
                var user = _userService.GetByMail(volunteerForRegisterDto.Email);
                user.FirstName = volunteerForRegisterDto.FirstName;
                user.LastName = volunteerForRegisterDto.LastName;
                _userService.Update(user);

               var volunteer= _volunteerDal.Get(x => x.UserId == user.Id);

                volunteer.CityId = volunteerForRegisterDto.CityId;
                volunteer.BirthDate = volunteerForRegisterDto.BirthDate;
                volunteer.CompanyId = volunteerForRegisterDto.CompanyId;
                volunteer.Gender = volunteerForRegisterDto.Gender;
                volunteer.Phone = volunteerForRegisterDto.Phone;
                volunteer.UniversityId = volunteerForRegisterDto.UniversityId;
                volunteer.CompanyDepartmentId = volunteerForRegisterDto.CompanyDepartmentId;
                volunteer.HighSchool = volunteerForRegisterDto.HighSchool;
                volunteer.UpdateDate = DateTime.Now;

                _volunteerDal.Update(volunteer);
                volunteer.User = user;
                return new SuccessDataResult<Volunteer>(volunteer, Messages.UserRegistered);
            }
            catch (Exception hata)
            {
                return new ErrorDataResult<Volunteer>(Messages.VolunteerAddError);
            }
        }
        public IResult EnrollAdvertisement(AdvertisementVolunteerDto advertisementVolunteerDto)
        {
            var advertisement = _advertisementDal.Get(x => x.AdvertisementId == advertisementVolunteerDto.AdvertisementId);

            if (advertisement == null || advertisement.AppStartDate > DateTime.Now || advertisement.AppEndDate < DateTime.Now)
                return new ErrorResult(Messages.EndDateError);

            if (_volunteerDal.IsAdvertisementEnroll(advertisementVolunteerDto.AdvertisementId)){
                return new ErrorResult(Messages.ErrorEnroll);
            }
            if (advertisementVolunteerDto.AdvertisementId > 0 && advertisementVolunteerDto.VolunteerId > 0)
            {
                AdvertisementVolunteer advertisementVolunteer = new AdvertisementVolunteer();
                advertisementVolunteer.VolunteerId = advertisementVolunteerDto.VolunteerId;
                advertisementVolunteer.AdvertisementId = advertisementVolunteerDto.AdvertisementId;
                advertisementVolunteer.UpdateDate = DateTime.Now;
                advertisementVolunteer.InsertDate = DateTime.Now;
                advertisementVolunteer.Status = true;
                _advertisementVolunteerDal.Add(advertisementVolunteer);
                return new SuccessResult(Messages.SuccessAdded);
            }
            else
            {
                return new ErrorResult(Messages.ErrorAdded);
            }
        }

        public IResult ComplatedAdvertisement(VolunteerAdvertisementComplatedDto volunteerAdvertisementComplatedDto)
        {
            var advertisementVolunter = _advertisementVolunteerDal.Get(x => x.AdvertisementId == volunteerAdvertisementComplatedDto.AdvertisementId&&x.VolunteerId==volunteerAdvertisementComplatedDto.VolunteerId); ;
            if (advertisementVolunter == null || volunteerAdvertisementComplatedDto.VolunteerId != advertisementVolunter.VolunteerId)
            {
                return new ErrorResult(Messages.Error);
            }
            if (volunteerAdvertisementComplatedDto.AdvertisementId > 0 && volunteerAdvertisementComplatedDto.TotalWork > 0)
            {
                VolunteerAdvertisementComplated volunteerAdvertisementComplated = new VolunteerAdvertisementComplated();
                volunteerAdvertisementComplated.AdvertisementVolunteerId = advertisementVolunter.Id;
                volunteerAdvertisementComplated.TotalWork = volunteerAdvertisementComplatedDto.TotalWork;
                volunteerAdvertisementComplated.Status = true;
                volunteerAdvertisementComplated.InsertDate = DateTime.Now;
                volunteerAdvertisementComplated.UpdateDate = DateTime.Now;
                volunteerAdvertisementComplated.ConfirmationStatus = 2;
                _volunteerAdvertisementComplatedDal.Add(volunteerAdvertisementComplated);
                return new SuccessResult(Messages.SuccessAdded);
            }
            else
            {
                return new ErrorResult(Messages.ErrorAdded);
            }
        }
        public IDataResult<List<AdvertisementListView>> GetActiveAdvertisementList(int volunteerId)
        {
            List<AdvertisementListView> result = new List<AdvertisementListView>();
            AdvertisementQuery advertisementQuery = new AdvertisementQuery();
            advertisementQuery.VolunteerId = volunteerId;
            advertisementQuery.Complated = false;

            var advertisementList = _volunteerDal.GetAdvertisementList(advertisementQuery);
            foreach (var item in advertisementList)
            {
                result.Add(new AdvertisementListView()
                {
                    AdvertisementId = item.AdvertisementId,
                    AdvertisementDesc = item.AdvertisementDesc,
                    AdvertisementTitle = item.AdvertisementTitle,
                    EndDate = item.EndDate,
                    IsOnline = item.IsOnline,
                    OrganisationId = item.OrganisationId,
                    OrganisationName = item.Organisation.OrganisationName,
                    StartDate = item.StartDate,
                    Record = true,
                    Status = item.Status
                });
            }
            return new SuccessDataResult<List<AdvertisementListView>>(result);

        }
        public IDataResult<List<AdvertisementListView>> GetComplatedAdvertisementList(int volunteerId)
        {
            List<AdvertisementListView> result = new List<AdvertisementListView>();
            AdvertisementQuery advertisementQuery = new AdvertisementQuery();
            advertisementQuery.VolunteerId = volunteerId;
            advertisementQuery.Complated = true;

            var advertisementList = _volunteerDal.GetAdvertisementList(advertisementQuery);
            foreach (var item in advertisementList)
            {
                result.Add(new AdvertisementListView()
                {
                    AdvertisementId = item.AdvertisementId,
                    AdvertisementDesc = item.AdvertisementDesc,
                    AdvertisementTitle = item.AdvertisementTitle,
                    EndDate = item.EndDate,
                    IsOnline = item.IsOnline,
                    OrganisationId = item.OrganisationId,
                    OrganisationName = item.Organisation.OrganisationName,
                    StartDate = item.StartDate,
                    Record=true,
                    Status=item.Status
                });
            }
            return new SuccessDataResult<List<AdvertisementListView>>(result);

        }

        public IDataResult<VolunteerDashboardModel> GeVolunteerDashboard(int volunteerId)
        {
            try
            {
                var volunteerDetail = _volunteerDal.GetVolunteerDashboard(volunteerId);
                VolunteerDashboardModel result = new VolunteerDashboardModel();
                result.TotalActiveProjectCount = volunteerDetail.AdvertisementVolunteers == null ? 0 : volunteerDetail.AdvertisementVolunteers.Where(x => !x.VolunteerAdvertisementComplateds.Any(y=>y.ConfirmationStatus == 1)&& x.Advertisement.EndDate>DateTime.Now).Count();
                result.TotalComplatedCount = volunteerDetail.AdvertisementVolunteers == null ? 0 : volunteerDetail.AdvertisementVolunteers.Where(x => x.VolunteerAdvertisementComplateds.Any(y => y.ConfirmationStatus == 1)).Count();
                result.TotalComplatedHours = volunteerDetail.AdvertisementVolunteers == null ? 0 : volunteerDetail.AdvertisementVolunteers.Sum(x => x.VolunteerAdvertisementComplateds.Where(y => y.ConfirmationStatus == 1).Sum(y=>y.TotalWork));
                result.TotalOrganisationCount = volunteerDetail.AdvertisementVolunteers == null ? 0 : volunteerDetail.AdvertisementVolunteers.Select(x => x.Advertisement).Select(x => x.Organisation).Distinct().ToList().Count;
                Dictionary<string, int> purposeList = new Dictionary<string, int>();
                Dictionary<string, int> categoryList = new Dictionary<string, int>();

                if (volunteerDetail.AdvertisementVolunteers != null)
                {
                    foreach (var item in volunteerDetail.AdvertisementVolunteers.Select(x=>x.Advertisement).Where(x => x.Status == true).SelectMany(x => x.AdvertisementPurposes.Select(y => y.Purpose.PurposeName)).ToList())
                    {
                        if (purposeList.Any(x => x.Key == item))
                        {
                            purposeList[item] += 1;
                        }
                        else
                        {
                            purposeList[item] = 1;
                        }
                    }
                    foreach (var item in volunteerDetail.AdvertisementVolunteers.Select(x => x.Advertisement).Where(x => x.Status == true).SelectMany(x => x.AdvertisementCategorys.Select(y => y.Category.CategoryName)).ToList())
                    {
                        if (categoryList.Any(x => x.Key == item))
                        {
                            categoryList[item] += 1;
                        }
                        else
                        {
                            categoryList[item] = 1;
                        }
                    }
                }
                result.PurposeCount = purposeList;
                result.CategoryCount = categoryList;
                return new SuccessDataResult<VolunteerDashboardModel>(result);
            }
            catch (Exception hata)
            {
                return new ErrorDataResult<VolunteerDashboardModel>(null, Messages.Error);
            }
        }
        public IResult UserExists(string email)
        {
            if (_userService.GetByMail(email) != null)
            {
                return new ErrorResult(Messages.UserAlreadyExists);
            }
            return new SuccessResult();
        }
    }
}
