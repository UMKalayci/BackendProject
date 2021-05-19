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
    public class AdvertisementManager : IAdvertisementService
    {
        private IUnitOfWork _unitOfWork;
        protected readonly IPaginationUriService _uriService;
        private IAdvertisementDal _advertisementDal;

        public AdvertisementManager(IUnitOfWork unitOfWork, IAdvertisementDal advertisementDal, IPaginationUriService uriService)
        {
            _unitOfWork = unitOfWork;
            _advertisementDal = advertisementDal;
            _uriService = uriService;
        }

        public IPaginationResult<List<AdvertisementListView>> GetList(AdvertisementQuery adversimentQuery, PaginationQuery paginationQuery = null)
        {
            var list = _advertisementDal.GetList(adversimentQuery, paginationQuery).ToList();
            List<AdvertisementListView> resultList = new List<AdvertisementListView>();
            foreach (var item in list)
            {
                resultList.Add(new AdvertisementListView()
                {
                  AdvertisementId=item.AdvertisementId,
                  AdvertisementTitle=item.AdvertisementTitle,
                  AdvertisementDesc=item.AdvertisementDesc,
                  StartDate=item.StartDate,
                  EndDate=item.EndDate,
                  IsOnline=item.IsOnline,
                  OrganisationId=item.OrganisationId,
                  OrganisationName=item.Organisation.OrganisationName
                }
                );
            };
            int count = _advertisementDal.GetCount(adversimentQuery);
            return PaginationExtensions.CreatePaginationResult<List<AdvertisementListView>>(resultList, true, paginationQuery, count, _uriService);
        }
    }
}
