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
    public class AdvertisementManager : IAdvertisementService
    {
        protected readonly IPaginationUriService _uriService;
        private IAdvertisementDal _advertisementDal;
        private IAdvertisementCategoryDal _advertisementCategoryDal;
        private IAdvertisementPurposeDal _advertisementPurposeDal;
        private IVolunteerDal _volunteerDal;
        private ICommentDal _commentDal;
        private IVolunteerAdvertisementComplatedDal volunteerAdvertisementComplatedDal;
        public AdvertisementManager(IVolunteerAdvertisementComplatedDal _volunteerAdvertisementComplatedDal, ICommentDal commentDal, IPaginationUriService uriService, IVolunteerDal volunteerDal, IAdvertisementDal advertisementDal, IAdvertisementCategoryDal advertisementCategoryDal, IAdvertisementPurposeDal advertisementPurposeDal)
        {
            _uriService = uriService;
            _advertisementDal = advertisementDal;
            _advertisementCategoryDal = advertisementCategoryDal;
            _advertisementPurposeDal = advertisementPurposeDal;
            _volunteerDal = volunteerDal;
            _commentDal = commentDal;
            _volunteerAdvertisementComplatedDal = volunteerAdvertisementComplatedDal;
        }

        public IDataResult<int> AddComment(CommentDto comment)
        {
            try
            {
                //var work = volunteerAdvertisementComplatedDal.Get(x => x.AdvertisementVolunteer.AdvertisementId == comment.AdvertisementId &&
                //                                      x.AdvertisementVolunteer.VolunteerId == comment.VolunteerId
                //                                      && x.ConfirmationStatus == 1);
                //if (work != null)
                //{

                    Comment c = new Comment();
                    c.AdvertisementId = comment.AdvertisementId;
                    c.VolunteerId = comment.VolunteerId;
                    c.Desc = comment.Desc;
                    c.InsertDate = DateTime.Now;
                    c.UpdateDate = DateTime.Now;
                    c.Status = true;
                    _commentDal.Add(c);
                    return new SuccessDataResult<int>(1, Messages.SuccessAdded);

                //}
                //else
                //{
                //    return new ErrorDataResult<int>(0, "Yorum yapabilmeniz için öncelikle çalışma yapmanız gerekmektedir.");
                //}
            }
            catch
            {
                return new ErrorDataResult<int>(0, Messages.ErrorAdded);

            }
        }
        public IPaginationResult<List<AdvertisementListView>> GetList(AdvertisementQuery adversimentQuery, PaginationQuery paginationQuery = null)
        {
            var list = _advertisementDal.GetList(adversimentQuery, paginationQuery).ToList();
            var volunteerAdvertisementList = _volunteerDal.GetAdvertisementList(adversimentQuery, null);
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
                    OrganisationName = item.Organisation.OrganisationName,
                    Record = volunteerAdvertisementList.Any(x => x.AdvertisementId == item.AdvertisementId)
                }
                );
            };
            int count = _advertisementDal.GetCount(adversimentQuery);
            return PaginationExtensions.CreatePaginationResult<List<AdvertisementListView>>(resultList, true, paginationQuery, count, _uriService);
        }
        public IPaginationResult<List<AdvertisementListView>> GetApproveList(AdminAdvertisementApproveQuery adversimentQuery, PaginationQuery paginationQuery = null)
        {
            var list = _advertisementDal.GetApproveList(adversimentQuery, paginationQuery).ToList();
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
                    OrganisationName = item.Organisation.OrganisationName,
                    Status = item.Status
                }
                );
            };
            int count = _advertisementDal.GetApproveCount(adversimentQuery);
            return PaginationExtensions.CreatePaginationResult<List<AdvertisementListView>>(resultList, true, paginationQuery, count, _uriService);
        }

        public IDataResult<AdvertisementDetailView> GetAdvertisementDetail(int advertisementId)
        {
            AdvertisementDetailView advertisementDetailView = new AdvertisementDetailView();

            var advertisement = _advertisementDal.GetDetail(advertisementId);
            advertisementDetailView.Name = advertisement.AdvertisementTitle;
            advertisementDetailView.Description = advertisement.AdvertisementDesc;
            advertisementDetailView.ApplicationEndDate = advertisement.AppEndDate;
            advertisementDetailView.EndDate = advertisement.EndDate;
            advertisementDetailView.StartDate = advertisement.StartDate;
            advertisementDetailView.ApplicationStartDate = advertisement.AppStartDate;
            advertisementDetailView.IsApplied = advertisement.IsApplied;
            advertisementDetailView.Location = advertisement.City.CityName;
            advertisementDetailView.ProjectImage = advertisement.Image;
            advertisementDetailView.Purposes = advertisement.AdvertisementPurposes.Select(x => x.Purpose.PurposeName).ToList();
            advertisementDetailView.Categories = advertisement.AdvertisementCategorys.Select(x => x.Category.CategoryName).ToList();
            advertisementDetailView.Corporation = advertisement.Organisation.OrganisationName;
            advertisementDetailView.ApplicantCount = advertisement.AdvertisementVolunteers.Count;
            advertisementDetailView.CommentList = new List<CommentView>();
            foreach (var item in advertisement.Comments)
            {
                advertisementDetailView.CommentList.Add(new CommentView()
                {
                    Desc = item.Desc,
                    Id = item.Id,
                    InsertDate = item.InsertDate,
                    VolunteerName = item.Volunteer.User.FirstName + " " + item.Volunteer.User.LastName
                });
            }
            return new SuccessDataResult<AdvertisementDetailView>(advertisementDetailView);
        }
        public IDataResult<int> Add(AdvertisementDto advertisementDto)
        {
            if (advertisementDto.OrganisationId != 0 &&
                advertisementDto.StartDate != DateTime.MinValue &&
                advertisementDto.EndDate != DateTime.MinValue &&
                advertisementDto.AdvertisementTitle != null &&
                advertisementDto.AdvertisementDesc != null &&
                advertisementDto.AppStartDate != DateTime.MinValue &&
                advertisementDto.AppEndDate != DateTime.MinValue &&
                advertisementDto.CityId != 0
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
                    advertisement.AppEndDate = advertisementDto.AppEndDate;
                    advertisement.AppStartDate = advertisementDto.AppStartDate;
                    advertisement.CityId = advertisementDto.CityId;
                    advertisement.IsApplied = advertisementDto.IsApplied;
                    advertisement.Image = advertisementDto.Image;
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
        public IResult ApproveAdvertisement(int advertisementId)
        {
            var advertisement = _advertisementDal.Get(x => x.AdvertisementId == advertisementId);
            if (advertisement != null)
            {
                try
                {
                    advertisement.Status = true;
                    advertisement.UpdateDate = DateTime.Now;
                    _advertisementDal.Update(advertisement);
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
