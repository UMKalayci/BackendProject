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
    public interface IAdvertisementService
    {
        IPaginationResult<List<AdvertisementListView>> GetList(AdvertisementQuery adversimentQuery, PaginationQuery paginationQuery = null);
        IDataResult<AdvertisementDetailView> GetAdvertisementDetail(int advertisementId);
        IDataResult<int> Add(AdvertisementDto advertisementDto);
        IResult ApproveAdvertisement(int advertisementId);
        IPaginationResult<List<AdvertisementListView>> GetApproveList(AdminAdvertisementApproveQuery adversimentQuery, PaginationQuery paginationQuery = null);
    }
}
