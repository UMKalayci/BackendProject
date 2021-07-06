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

        public IPaginationResult<List<OrganisationApproveListView>> GetApproveList(OrganisationListQuery organisationListQuery, PaginationQuery paginationQuery = null)
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
                    Status=item.Status
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
                    AdvertisementName=item.AdvertisementVolunteer.Advertisement.AdvertisementTitle,
                    VolunteerName=item.AdvertisementVolunteer.Volunteer.User.FirstName + " " +item.AdvertisementVolunteer.Volunteer.User.LastName,
                    ConfirmationStatus=item.ConfirmationStatus,
                    TotalWork=item.TotalWork,
                    ComplatedId=item.Id
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
    }
}
