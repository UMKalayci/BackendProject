using Business.Abstract;
using Entities.QueryModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private IOrganisationService _organisationService;
        public AdminController(IOrganisationService organisationService)
        {
            _organisationService = organisationService;
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("GetList")]
        public ActionResult GetList([FromQuery] OrganisationListQuery organisationListQuery, [FromQuery] PaginationQuery paginationQuery)
        {
            var result = _organisationService.GetApproveList(organisationListQuery, paginationQuery);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }
    }
}
