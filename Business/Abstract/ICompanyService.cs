using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using Entities.QueryModels;
using Entities.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface ICompanyService
    {
        IDataResult<List<Company>> GetList();
        IDataResult<Company> Register(CompanyForRegisterDto companyForRegisterDto, string password);
        IResult UserExists(string email);
        IDataResult<Company> GetCompany(int userId);
    }
}
