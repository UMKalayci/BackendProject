using System;
using System.Collections.Generic;
using System.Linq;
using Business.Abstract;
using Business.BusinessAspects.Pagination;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.QueryModels;
using Entities.Views;

namespace Business.Concrete
{
    public class PurposeManager : IPurposeService
    {
        protected readonly IPaginationUriService _uriService;
        private IPurposeDal _purposeDal;
        public PurposeManager(IPurposeDal purposeDal)
        {
            _purposeDal = purposeDal;
        }

        public IDataResult<List<GlobalPurpose>> GetList()
        {
            return new SuccessDataResult<List<GlobalPurpose>> (_purposeDal.GetList().OrderBy(x=>x.PurposeName).ToList());
        }


    }
}
