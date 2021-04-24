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
    public class VolunteerManager:IVolunteerService
    {
        private IUserService _userService;
        private IVolunteerDal _volunteerDal;

        public VolunteerManager(IUserService userService, IVolunteerDal volunteerDal)
        {
            _volunteerDal = volunteerDal;
            _userService = userService;
        }

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
            try { 
            _userService.Add(user);
            int userId = _userService.GetByMail(user.Email).Id;

            var volunter = new Volunteer
            {
                UserId=userId,
                CityId = volunteerForRegisterDto.CityId,
                BirthDate = volunteerForRegisterDto.BirthDate,
                CompanyId = volunteerForRegisterDto.CompanyId,
                Gender = volunteerForRegisterDto.Gender,
                Phone = volunteerForRegisterDto.Phone,
                UniversityId = volunteerForRegisterDto.UniversityId,
                UpdateDate = DateTime.Now,
                InsertDate = DateTime.Now,
                Status = true
            };

            _volunteerDal.Add(volunter);

            return new SuccessDataResult<Volunteer>(volunter, Messages.UserRegistered);
            }
            catch
            {
                return new ErrorDataResult<Volunteer>(Messages.VolunteerAddError);
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
