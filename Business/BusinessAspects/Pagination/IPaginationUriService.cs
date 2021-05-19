using Entities.QueryModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.BusinessAspects.Pagination
{
    public interface IPaginationUriService
    {
         Uri GetPageUri(PaginationQuery paginationQuery);
    }
}
