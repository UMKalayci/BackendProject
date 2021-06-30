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
    public class CategoryManager : ICategoryService
    {
        protected readonly IPaginationUriService _uriService;
        private ICategoryDal _categoryDal;
        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public IDataResult<List<Category>> GetList()
        {
            return new SuccessDataResult<List<Category>>(_categoryDal.GetList().OrderBy(x=>x.CategoryName).ToList());
        }


    }
}
