using Business.Abstract;
using Entities.Dtos;
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
        private IAdvertisementService _advertisementService;
        private ICompanyService _companyService;
        public AdminController(ICompanyService companyService,IOrganisationService organisationService, IAdvertisementService advertisementService)
        {
            _companyService = companyService;
            _organisationService = organisationService;
            _advertisementService = advertisementService;
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("GetOrganisationList")]
        public ActionResult GetOrganisationList([FromQuery] AdminOrganisationListQuery organisationListQuery, [FromQuery] PaginationQuery paginationQuery)
        {
            var result = _organisationService.GetApproveList(organisationListQuery, paginationQuery);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("GetAdvertisementList")]
        public ActionResult GetAdvertisementList([FromQuery] AdminAdvertisementApproveQuery advertisementQuery, [FromQuery] PaginationQuery paginationQuery)
        {
            var result = _advertisementService.GetApproveList(advertisementQuery, paginationQuery);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("GetCompanyList")]
        public ActionResult GetCompanyList([FromQuery] AdminCompanyApproveQuery advertisementQuery, [FromQuery] PaginationQuery paginationQuery)
        {
            var result = _companyService.GetApproveList(advertisementQuery, paginationQuery);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }



        [Authorize(Policy = "AdminOnly")]
        [HttpPost("ApproveCompany")]
        public ActionResult ApproveCompany(CompanyApproveDto companyApproveDto)
        {
            var result = _companyService.ApproveCompany(companyApproveDto.CompanyId);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("ApproveAdvertisement")]
        public ActionResult ApproveAdvertisement(AdvertisementApproveDto advertisementApproveDto)
        {
            var result = _advertisementService.ApproveAdvertisement(advertisementApproveDto.AdvertisementId);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("ApproveOrganisation")]
        public ActionResult ApproveOrganisation( OrganisationApproveDto organisationApproveDto)
        {
            var result = _organisationService.ApproveOrganisation(organisationApproveDto.OrganisationId);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }

    }
}
