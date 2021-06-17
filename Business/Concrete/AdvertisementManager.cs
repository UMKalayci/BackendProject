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
        protected readonly IPaginationUriService _uriService;
        private IAdvertisementDal _advertisementDal;
        private IAdvertisementCategoryDal _advertisementCategoryDal;
        private IAdvertisementPurposeDal _advertisementPurposeDal;
        public AdvertisementManager( IPaginationUriService uriService, IAdvertisementDal advertisementDal, IAdvertisementCategoryDal advertisementCategoryDal, IAdvertisementPurposeDal advertisementPurposeDal)
        {
            _uriService = uriService;
            _advertisementDal = advertisementDal;
            _advertisementCategoryDal = advertisementCategoryDal;
            _advertisementPurposeDal = advertisementPurposeDal;
        }

        public IPaginationResult<List<AdvertisementListView>> GetList(AdvertisementQuery adversimentQuery, PaginationQuery paginationQuery = null)
        {
            var list = _advertisementDal.GetList(adversimentQuery, paginationQuery).ToList();
            List<AdvertisementListView> resultList = new List<AdvertisementListView>();
            foreach (var item in list)
            {
                resultList.Add(new AdvertisementListView()
                {
                    AdvertisementId = item.AdvertisementId,
                    AdvertisementTitle = item.AdvertisementTitle,
                    AdvertisementDesc = item.AdvertisementDesc,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    IsOnline = item.IsOnline,
                    OrganisationId = item.OrganisationId,
                    OrganisationName = item.Organisation.OrganisationName
                }
                );
            };
            int count = _advertisementDal.GetCount(adversimentQuery);
            return PaginationExtensions.CreatePaginationResult<List<AdvertisementListView>>(resultList, true, paginationQuery, count, _uriService);
        }
        public IDataResult<int> Add(AdvertisementDto advertisementDto)
        {
            if (advertisementDto.OrganisationId != 0 &&
                advertisementDto.StartDate != DateTime.MinValue &&
                advertisementDto.EndDate != DateTime.MinValue &&
                advertisementDto.AdvertisementTitle != null &&
                advertisementDto.AdvertisementDesc != null
                )
            {
                try
                {
                    DateTime now = DateTime.Now;
                    Advertisement advertisement = new Advertisement();
                    advertisement.AdvertisementTitle = advertisementDto.AdvertisementTitle;
                    advertisement.AdvertisementDesc = advertisementDto.AdvertisementDesc;
                    advertisement.OrganisationId = advertisementDto.OrganisationId;
                    advertisement.StartDate = advertisementDto.StartDate;
                    advertisement.EndDate = advertisementDto.EndDate;
                    advertisement.InsertDate = now;
                    advertisement.UpdateDate = now;
                    advertisement.Status = false;
                    _advertisementDal.Add(advertisement);

                    int advertisementId = _advertisementDal.Get(x => x.InsertDate == now && x.UpdateDate == now && x.AdvertisementTitle == advertisement.AdvertisementTitle).AdvertisementId;

                    foreach (var item in advertisementDto.CategoryIdList)
                    {
                        AdvertisementCategory advertisementCategory = new AdvertisementCategory();
                        advertisementCategory.AdvertisementId = advertisementId;
                        advertisementCategory.CategoryId = item;
                        advertisementCategory.InsertDate = now;
                        advertisementCategory.UpdateDate = now;
                        advertisementCategory.Status = true;
                        _advertisementCategoryDal.Add(advertisementCategory);
                    }

                    foreach (var item in advertisementDto.PurposeIdList)
                    {
                        AdvertisementPurpose advertisementPurpose = new AdvertisementPurpose();
                        advertisementPurpose.AdvertisementId = advertisementId;
                        advertisementPurpose.PurposeId = item;
                        advertisementPurpose.InsertDate = now;
                        advertisementPurpose.UpdateDate = now;
                        advertisementPurpose.Status = true;
                        _advertisementPurposeDal.Add(advertisementPurpose);
                    }
                    return new SuccessDataResult<int>(advertisementId, Messages.SuccessAdded);
                }
                catch
                {
                    return new ErrorDataResult<int>(-1, Messages.ErrorAdded);
                }
            }
            else
            {
                return new ErrorDataResult<int>(-1, Messages.MissingFieldError);

            }
        }
    }
}
