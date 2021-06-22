﻿using System;
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
    public class VolunteerManager : IVolunteerService
    {
        private IUserService _userService;
        private IAdvertisementVolunteerDal _advertisementVolunteerDal;
        private IVolunteerDal _volunteerDal;

        public VolunteerManager(IUserService userService, IAdvertisementVolunteerDal advertisementVolunteerDal, IVolunteerDal volunteerDal)
        {
            _advertisementVolunteerDal = advertisementVolunteerDal;
             _userService = userService;
            _volunteerDal = volunteerDal;
        }
        public IDataResult<Volunteer> GetVolunteer(int userId)
        {
            var volunteer = _volunteerDal.Get(x => x.UserId == userId);
            if (volunteer != null)
                return new SuccessDataResult<Volunteer>(volunteer);
            else
                return new ErrorDataResult<Volunteer>(null, Messages.UserNotFound);
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
            catch(Exception hata)
            {
                return new ErrorDataResult<Volunteer>(Messages.VolunteerAddError);
            }
        }
        public IResult EnrollAdvertisement(AdvertisementVolunteerDto advertisementVolunteerDto)
        {
            if (advertisementVolunteerDto.AdvertisementId != 0 && advertisementVolunteerDto.VolunteerId != 0)
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
