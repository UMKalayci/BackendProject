using System;
using System.Collections.Generic;
using System.Linq;
using Business.Abstract;
using Business.BusinessAspects.Pagination;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.QueryModels;
using Entities.Views;

namespace Business.Concrete
{
    public class CompanyManager : ICompanyService
    {
        protected readonly IPaginationUriService _uriService;
        private IUserService _userService;
        private ICompanyDal _companyDal;
        public CompanyManager(IUserService userService ,ICompanyDal companyDal)
        {
            _companyDal = companyDal;
            _userService = userService;
        }

        public IDataResult<List<Company>> GetList()
        {
            return new SuccessDataResult<List<Company>>(_companyDal.GetList().OrderBy(x=>x.CompanyName).ToList());
        }

        public IDataResult<Company> Register(CompanyForRegisterDto companyForRegisterDto, string password)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var user = new User
            {
                Email = companyForRegisterDto.Email,
                FirstName = companyForRegisterDto.FirstName,
                LastName = companyForRegisterDto.LastName,
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
                userOperationClaim.OperationClaimId = 3;//KSS
                _userService.AddUserClaims(userOperationClaim);

                var company = new Company
                {
                    UserId = userId,
                    CityId= companyForRegisterDto.CityId,
                    CompanyName= companyForRegisterDto.CompanyName,
                    Desc= companyForRegisterDto.Desc,
                    Image= companyForRegisterDto.Image,
                    FoundationOfYear= companyForRegisterDto.FoundationOfYear,
                    Phone= companyForRegisterDto.Phone,
                    UpdateDate = DateTime.Now,
                    InsertDate = DateTime.Now,
                    Status = false
                };

                _companyDal.Add(company);
                company.User = user;
                return new SuccessDataResult<Company>(company, Messages.UserRegistered);
            }
            catch (Exception hata)
            {
                return new ErrorDataResult<Company>(Messages.ErrorAdded);
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
        public IDataResult<Company> GetCompany(int userId)
        {
            var company = _companyDal.Get(x => x.UserId == userId);
            if (company != null)
                return new SuccessDataResult<Company>(company);
            else
                return new ErrorDataResult<Company>(null, Messages.UserNotFound);
        }
        public List<VolunteerListView> GetCompanyVolunteerList(int companyId)
        {
            List<VolunteerListView> result = new List<VolunteerListView>();
            var volunteerList=_companyDal.GetCompanyVolunteerList(companyId);
            foreach (var item in volunteerList)
            {
                result.Add(new VolunteerListView()
                {
                    BirthDate=item.BirthDate,
                    CityName=item.City.CityName,
                    Gender=item.Gender,
                    NameSurname=item.User.FirstName + " "+ item.User.LastName,
                    Phone=item.Phone,
                    VolunteerId=item.VolunteerId
                });
            }

            return result;
        }



    }
}
