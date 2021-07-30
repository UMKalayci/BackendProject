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
        public CompanyManager(IPaginationUriService uriService,IUserService userService, ICompanyDal companyDal)
        {
            _companyDal = companyDal;
            _userService = userService;
            _uriService = uriService;
        }

        public IPaginationResult<List<CompanyView>> GetApproveList(AdminCompanyApproveQuery companyQuery, PaginationQuery paginationQuery)
        {
            var list = _companyDal.GetApproveList(companyQuery, paginationQuery).ToList();
            List<CompanyView> resultList = new List<CompanyView>();
            foreach (var item in list)
            {
                resultList.Add(new CompanyView()
                {
                  CityName=item.City.CityName,
                  CompanyId=item.CompanyId,
                  CompanyName=item.CompanyName,
                  Desc=item.Desc,
                  FoundationOfYear=item.FoundationOfYear,
                  Image=item.Image,
                  Phone=item.Phone,
                  Status=item.Status
                });
            };
            int count = _companyDal.GetApproveCount(companyQuery);
            return PaginationExtensions.CreatePaginationResult<List<CompanyView>>(resultList, true, paginationQuery, count, _uriService);

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
                    CityId = companyForRegisterDto.CityId,
                    CompanyName = companyForRegisterDto.CompanyName,
                    Desc = companyForRegisterDto.Desc,
                    Image = companyForRegisterDto.Image,
                    FoundationOfYear = companyForRegisterDto.FoundationOfYear,
                    Phone = companyForRegisterDto.Phone,
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
        public IDataResult<List<VolunteerListView>> GetCompanyVolunteerList(int companyId)
        {
            List<VolunteerListView> result = new List<VolunteerListView>();
            var volunteerList = _companyDal.GetCompanyVolunteerList(companyId);
            foreach (var item in volunteerList)
            {
                result.Add(new VolunteerListView()
                {
                    BirthDate = item.BirthDate,
                    CityName = item.City.CityName,
                    Gender = item.Gender,
                    NameSurname = item.User.FirstName + " " + item.User.LastName,
                    Phone = item.Phone,
                    VolunteerId = item.VolunteerId
                });
            }

            return new SuccessDataResult<List<VolunteerListView>>(result);
        }
        public IDataResult<List<CompanyComboListView>> GetComboList()
        {
            List<CompanyComboListView> result = new List<CompanyComboListView>();
            var companyList = _companyDal.GetList(x => x.Status == true).OrderBy(x => x.CompanyName);
            foreach (var item in companyList)
            {
                result.Add(new CompanyComboListView()
                {
                    CompanyId = item.CompanyId,
                    CompanyName = item.CompanyName
                });
            }

            return new SuccessDataResult<List<CompanyComboListView>>(result);
        }
        public IDataResult<CompanyView> GetProfilDetail(int companyId)
        {
            CompanyView result = new CompanyView();
            var company = _companyDal.GetCompanyProfilDetail(companyId);

            result.CityName = company.City.CityName;
            result.CompanyId = company.CompanyId;
            result.CompanyName = company.CompanyName;
            result.FoundationOfYear = company.FoundationOfYear;
            result.Phone = company.Phone;
            result.Image = company.Image;
            result.Desc = company.Desc;
            return new SuccessDataResult<CompanyView>(result);
        }
        public IDataResult<CompanyDashboardView> GetCompanyDashboard(int companyId)
        {
            CompanyDashboardView result = new CompanyDashboardView();
            var company = _companyDal.GetCompanyDashboard(companyId);

            result.VolunteerCount = company.Volunteers == null ? 0 : company.Volunteers.Count;
            var a = company.Volunteers.SelectMany(x => x.AdvertisementVolunteers);
            if (company.Volunteers != null) {
                result.VolunteerProjectCount = company.Volunteers.SelectMany(x => x.AdvertisementVolunteers) == null ? 0 : company.Volunteers.SelectMany(x => x.AdvertisementVolunteers).ToList().Count;
            }
            else
            {
                result.VolunteerProjectCount = 0;
            }

            if (company.Volunteers != null)
            {
                result.VolunteerComplatedCount = company.Volunteers.SelectMany(x => x.AdvertisementVolunteers.SelectMany(y=>y.VolunteerAdvertisementComplateds.Where(z=>z.ConfirmationStatus==1))) == null ? 0 : company.Volunteers.SelectMany(x => x.AdvertisementVolunteers.SelectMany(y => y.VolunteerAdvertisementComplateds.Where(z => z.ConfirmationStatus == 1))).ToList().Count;
            }
            else
            {
                result.VolunteerComplatedCount = 0;
            }


            if (company.Volunteers != null)
            {
                result.VolunteerTotalWorkHours = company.Volunteers.SelectMany(x => x.AdvertisementVolunteers.SelectMany(y => y.VolunteerAdvertisementComplateds.Where(z => z.ConfirmationStatus == 1))) == null ? 0 : company.Volunteers.Sum(x => x.AdvertisementVolunteers.Sum(y => y.VolunteerAdvertisementComplateds.Where(z => z.ConfirmationStatus == 1).Sum(t=>t.TotalWork)));
            }
            else
            {
                result.VolunteerTotalWorkHours = 0;
            }
            return new SuccessDataResult<CompanyDashboardView>(result);
        }

        public IResult ApproveCompany(int companyId)
        {
            var company = _companyDal.Get(x => x.CompanyId == companyId);
            if (company != null)
            {
                try
                {
                    company.Status = true;
                    company.UpdateDate = DateTime.Now;
                    _companyDal.Update(company);
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
