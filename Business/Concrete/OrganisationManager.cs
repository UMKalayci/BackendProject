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
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Business.Concrete
{
    public class OrganisationManager : IOrganisationService
    {
        private IUserService _userService;
        private IOrganisationDal _organisationDal;

        public OrganisationManager(IUserService userService, IOrganisationDal organisationDal)
        {
            _userService = userService;
            _organisationDal = organisationDal;
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
                    Status = true
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
