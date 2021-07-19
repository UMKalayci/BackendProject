using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.BusinessAspects.Pagination;
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
    public class OrganisationManager : IOrganisationService
    {
        protected readonly IPaginationUriService _uriService;
        private IUserService _userService;
        private IVolunteerAdvertisementComplatedDal _volunteerAdvertisementComplatedDal;
        private IOrganisationDal _organisationDal;
        private IAdvertisementVolunteerDal _advertisementVolunteerDal;
        private IAdvertisementDal _advertisementDal;

        public OrganisationManager(IPaginationUriService uriService, IUserService userService, IVolunteerAdvertisementComplatedDal volunteerAdvertisementComplatedDal, IAdvertisementVolunteerDal advertisementVolunteerDal, IAdvertisementDal advertisementDal, IOrganisationDal organisationDal)
        {
            _uriService = uriService;
            _userService = userService;
            _organisationDal = organisationDal;
            _advertisementDal = advertisementDal;
            _volunteerAdvertisementComplatedDal = volunteerAdvertisementComplatedDal;

            _advertisementVolunteerDal = advertisementVolunteerDal;
        }

        public IPaginationResult<List<VolunteerListView>>  GetOrganisationVolunteerList(int organisationId, PaginationQuery paginationQuery = null)
        {
            try
            {
                var volunteerList = _organisationDal.GetOrganisationVolunteerList(organisationId);
                List<VolunteerListView> resultList = new List<VolunteerListView>();
                foreach (var item in volunteerList)
                {
                    resultList.Add(new VolunteerListView()
                    {
                        BirthDate=item.BirthDate,
                        CityName=item.City.CityName,
                        Gender=item.Gender,
                        NameSurname=item.User.FirstName+" "+item.User.LastName,
                        Phone=item.Phone,
                        VolunteerId=item.VolunteerId
                    });;
                }
                int count = _organisationDal.GetOrganisationVolunteerCount(organisationId);
                return PaginationExtensions.CreatePaginationResult<List<VolunteerListView>>(resultList, true, paginationQuery, count, _uriService);
            }
            catch (Exception hata)
            {
                return null;
            }
        }


        public IDataResult<OrganisationDashboardModel> GetOrganisationDashboard(int organisationId)
        {
            try
            {
                var organisationDetail = _organisationDal.GetOrganisationDashboard(organisationId);
                OrganisationDashboardModel result = new OrganisationDashboardModel();
                result.TotalActiveProjectCount = organisationDetail.Advertisements.Count();
                result.TotalVolunteerCount = organisationDetail.Advertisements.Sum(x => x.AdvertisementVolunteers.Count);
                result.TotalVolunteerWorkCount = organisationDetail.Advertisements.Sum(x => x.AdvertisementVolunteers.Sum(y => y.VolunteerAdvertisementComplateds.Sum(z => z.TotalWork)));
                Dictionary<string, int> purposeList = new Dictionary<string, int>();
                foreach (var item in organisationDetail.Advertisements.SelectMany(x=>x.AdvertisementPurposes.Select(y=>y.Purpose.PurposeName)).ToList())
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
                Dictionary<string, int> categoryList = new Dictionary<string, int>();
                foreach (var item in organisationDetail.Advertisements.SelectMany(x => x.AdvertisementCategorys.Select(y => y.Category.CategoryName)).ToList())
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
                result.PurposeCount = purposeList;
                result.CategoryCount = categoryList;
                return new SuccessDataResult<OrganisationDashboardModel>(result);
            }
            catch (Exception hata)
            {
                return new ErrorDataResult<OrganisationDashboardModel>(null, Messages.Error);
            }
        }
        public IPaginationResult<List<OrganisationApproveListView>> GetApproveList(AdminOrganisationListQuery organisationListQuery, PaginationQuery paginationQuery = null)
        {
            var list = _organisationDal.GetApproveList(organisationListQuery, paginationQuery).ToList();
            List<OrganisationApproveListView> resultList = new List<OrganisationApproveListView>();
            foreach (var item in list)
            {
                resultList.Add(new OrganisationApproveListView()
                {
                    City = item.City.CityName,
                    Desc = item.Desc,
                    FoundationOfYear = item.FoundationOfYear,
                    IsMemberAcikAcik = item.IsMemberAcikAcik,
                    OrganisationId = item.OrganisationId,
                    OrganisationName = item.OrganisationName,
                    Phone = item.Phone,
                    Status = item.Status
                });
            };
            int count = _organisationDal.GetApproveCount(organisationListQuery);
            return PaginationExtensions.CreatePaginationResult<List<OrganisationApproveListView>>(resultList, true, paginationQuery, count, _uriService);
        }

        public IDataResult<Organisation> GetOrganisation(int userId)
        {
            var organisation = _organisationDal.Get(x => x.UserId == userId);
            if (organisation != null)
                return new SuccessDataResult<Organisation>(organisation);
            else
                return new ErrorDataResult<Organisation>(null, Messages.UserNotFound);
        }
        public IDataResult<Organisation> Register(OrganisationForRegisterDto organisationForRegisterDto, string password)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var user = new User
            {
                Email = organisationForRegisterDto.Email,
                FirstName = organisationForRegisterDto.FirstName,
                LastName = organisationForRegisterDto.LastName,
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
                userOperationClaim.OperationClaimId = 2;//STK
                _userService.AddUserClaims(userOperationClaim);

                var organisation = new Organisation
                {
                    UserId = userId,
                    CityId = organisationForRegisterDto.CityId,
                    Phone = organisationForRegisterDto.Phone,
                    FinanceDocument = organisationForRegisterDto.FinanceDocument,
                    Image = organisationForRegisterDto.Image,
                    Desc = organisationForRegisterDto.Desc,
                    FoundationOfYear = organisationForRegisterDto.FoundationOfYear,
                    IsMemberAcikAcik = organisationForRegisterDto.IsMemberAcikAcik,
                    OrganisationName = organisationForRegisterDto.OrganisationName,
                    UpdateDate = DateTime.Now,
                    InsertDate = DateTime.Now,
                    Status = false
                };

                _organisationDal.Add(organisation);
                organisation.User = user;
                return new SuccessDataResult<Organisation>(organisation, Messages.UserRegistered);
            }
            catch (Exception hata)
            {
                return new ErrorDataResult<Organisation>(Messages.SuccessAdded);
            }
        }

        public IDataResult<Organisation> Update(OrganisationForRegisterDto organisationForRegisterDto, string password)
        {
            try
            {
                var user = _userService.GetByMail(organisationForRegisterDto.Email);

                user.FirstName = organisationForRegisterDto.FirstName;
                user.LastName = organisationForRegisterDto.LastName;

                _userService.Update(user);
                var organisation = _organisationDal.Get(x=>x.UserId==user.Id);

                organisation.CityId = organisationForRegisterDto.CityId;
                organisation.Phone = organisationForRegisterDto.Phone;
                organisation.FinanceDocument = organisationForRegisterDto.FinanceDocument;
                organisation.Image = organisationForRegisterDto.Image;
                organisation.Desc = organisationForRegisterDto.Desc;
                organisation.FoundationOfYear = organisationForRegisterDto.FoundationOfYear;
                organisation.IsMemberAcikAcik = organisationForRegisterDto.IsMemberAcikAcik;
                organisation.OrganisationName = organisationForRegisterDto.OrganisationName;
                organisation.UpdateDate = DateTime.Now;

                _organisationDal.Update(organisation);
                organisation.User = user;
                return new SuccessDataResult<Organisation>(organisation, Messages.UserRegistered);
            }
            catch (Exception hata)
            {
                return new ErrorDataResult<Organisation>(Messages.SuccessAdded);
            }
        }
        public IResult ComplatedAdvertisementApprove(VolunteerAdvertisementComplatedApproveDto volunteerAdvertisementComplatedApproveDto)
        {
            try
            {
                var approveRecord = _volunteerAdvertisementComplatedDal.Get(x => x.Id == volunteerAdvertisementComplatedApproveDto.ComplatedId);

                var advertisementVolunter = _advertisementVolunteerDal.Get(x => x.Id == approveRecord.AdvertisementVolunteerId);
                var advertisement = _advertisementDal.Get(x => x.AdvertisementId == advertisementVolunter.AdvertisementId);
                if (advertisement.OrganisationId == volunteerAdvertisementComplatedApproveDto.OrganisationId)
                {
                    approveRecord.ConfirmationStatus = volunteerAdvertisementComplatedApproveDto.Approve;
                    approveRecord.UpdateDate = DateTime.Now;
                    _volunteerAdvertisementComplatedDal.Update(approveRecord);
                    return new SuccessResult(Messages.SuccessAdded);

                }
                else
                {
                    return new ErrorResult(Messages.Error);
                }

            }
            catch
            {
                return new ErrorResult(Messages.Error);
            }
        }
        public IDataResult<List<ApproveListView>> AdvertisementApproveList(int organisationId)
        {
            var list = _volunteerAdvertisementComplatedDal.GetApproveListByOrganisationId(organisationId);
            List<ApproveListView> resultList = new List<ApproveListView>();
            foreach (var item in list)
            {
                resultList.Add(new ApproveListView()
                {
                    AdvertisementName = item.AdvertisementVolunteer.Advertisement.AdvertisementTitle,
                    VolunteerName = item.AdvertisementVolunteer.Volunteer.User.FirstName + " " + item.AdvertisementVolunteer.Volunteer.User.LastName,
                    ConfirmationStatus = item.ConfirmationStatus,
                    TotalWork = item.TotalWork,
                    ComplatedId = item.Id
                });
            }

            return new SuccessDataResult<List<ApproveListView>>(resultList);
        }
        public IResult UserExists(string email)
        {
            if (_userService.GetByMail(email) != null)
            {
                return new ErrorResult(Messages.UserAlreadyExists);
            }
            return new SuccessResult();
        }

        public IResult ApproveOrganisation(int organisationId)
        {
            var organisation = _organisationDal.Get(x => x.OrganisationId == organisationId);
            if (organisation != null)
            {
                try
                {
                    organisation.Status = true;
                    organisation.UpdateDate = DateTime.Now;
                    _organisationDal.Update(organisation);
                    return new SuccessResult(Messages.SuccessAdded);
                }
                catch
                {
                    return new ErrorResult(Messages.ErrorAdded);
                }
            }
            return new ErrorResult(Messages.ErrorAdded);
        }
    }
}
