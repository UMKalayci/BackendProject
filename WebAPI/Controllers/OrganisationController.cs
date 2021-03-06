using Business.Abstract;
using Business.Constants;
using Entities.Dtos;
using Entities.QueryModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganisationController : ControllerBase
    {
        private IOrganisationService _organisationService;
        private IAdvertisementService _advertisementService;
        private IAuthService _authService;
        public OrganisationController(IAdvertisementService advertisementService, IOrganisationService organisationService, IAuthService authService)
        {
            _organisationService = organisationService;
            _authService = authService;
            _advertisementService = advertisementService;
        }
        //tamam
        [HttpPost("register")]
        public ActionResult Register(OrganisationForRegisterDto organisationForRegisterDto)
        {
            var userExists = _organisationService.UserExists(organisationForRegisterDto.Email);
            if (!userExists.Success)
            {
                return BadRequest(userExists.Message);
            }

            var registerResult = _organisationService.Register(organisationForRegisterDto, organisationForRegisterDto.Password);
            if (registerResult.Success)
            {
                var result = _authService.CreateAccessToken(registerResult.Data.User);

                if (result.Success)
                {
                    return Ok(result.Data);
                }
                return BadRequest(result.Message);
            }

            return BadRequest(registerResult.Message);
        }

        //tamam
        [Authorize(Policy = "OrganisationOnly")]
        [HttpPost("update")]
        public ActionResult Update(OrganisationForRegisterDto organisationForRegisterDto)
        {
            var userExists = _organisationService.UserExists(organisationForRegisterDto.Email);
            if (userExists.Success)
            {
                return BadRequest(userExists.Message);
            }

            var updateResult = _organisationService.Update(organisationForRegisterDto, organisationForRegisterDto.Password);
            if (updateResult.Success)
            {

                    return Ok(updateResult.Data);
            }

            return BadRequest(updateResult.Message);
        }

        //tamam
        [Authorize(Policy = "OrganisationOnly")]
        [HttpPost("ComplatedAdvertisementApprove")]
        public ActionResult ComplatedAdvertisementApprove(VolunteerAdvertisementComplatedApproveDto volunteerAdvertisementComplatedApproveDto)
        {
            var userID = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var organisation = _organisationService.GetOrganisation(Convert.ToInt32(userID));
            if (organisation.Data == null)
            {
                return BadRequest("STK bulunumadı!");
            }
            volunteerAdvertisementComplatedApproveDto.OrganisationId = organisation.Data.OrganisationId;

            var result = _organisationService.ComplatedAdvertisementApprove(volunteerAdvertisementComplatedApproveDto);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        //tamam
        [Authorize(Policy = "OrganisationOnly")]
        [HttpGet("AdvertisementApproveList")]
        public ActionResult AdvertisementApproveList()
        {
            var userID = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var organisation = _organisationService.GetOrganisation(Convert.ToInt32(userID));
            if (organisation.Data == null)
            {
                return BadRequest("STK bulunumadı!");
            }
            var result = _organisationService.AdvertisementApproveList(organisation.Data.OrganisationId);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        //tamam
        [Authorize(Policy = "OrganisationOnly")]
        [HttpGet("GetOrganisationDashboard")]
        public ActionResult GetOrganisationDashboard()
        {
            var userID = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var organisation = _organisationService.GetOrganisation(Convert.ToInt32(userID));
            if (organisation.Data == null)
            {
                return BadRequest("STK bulunumadı!");
            }
            var result = _organisationService.GetOrganisationDashboard(organisation.Data.OrganisationId);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }
        //tamam
        [Authorize(Policy = "OrganisationOnly")]
        [HttpGet("GetOrganisationVolunteerList")]
        public ActionResult GetOrganisationVolunteerList([FromQuery] PaginationQuery paginationQuery)
        {
            var userID = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var organisation = _organisationService.GetOrganisation(Convert.ToInt32(userID));
            if (organisation.Data == null)
            {
                return BadRequest("STK bulunumadı!");
            }
            var result = _organisationService.GetOrganisationVolunteerList(organisation.Data.OrganisationId, paginationQuery);
            if (result != null)
            {
                return Ok(result.Data);
            }
            return BadRequest(Messages.Error);
        }

        //tamam
        [Authorize(Policy = "OrganisationOnly")]
        [HttpGet("GetOrganisationAdvertisementList")]
        public ActionResult GetOrganisationAdvertisementList([FromQuery] PaginationQuery paginationQuery)
        {
            var userID = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var organisation = _organisationService.GetOrganisation(Convert.ToInt32(userID));
            if (organisation.Data == null)
            {
                return BadRequest("STK bulunumadı!");
            }
            var result = _advertisementService.GetList(new AdvertisementQuery() { OrganisationId = organisation.Data.OrganisationId,Complated=false }, paginationQuery);
            if (result != null)
            {
                return Ok(result.Data);
            }
            return BadRequest(Messages.Error);
        }

        //tamam
        [Authorize(Policy = "OrganisationOnly")]
        [HttpGet("GetOrganisationAdvertisementComplatedList")]
        public ActionResult GetOrganisationAdvertisementComplatedList([FromQuery] PaginationQuery paginationQuery)
        {
            var userID = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var organisation = _organisationService.GetOrganisation(Convert.ToInt32(userID));
            if (organisation.Data == null)
            {
                return BadRequest("STK bulunumadı!");
            }
            var result = _advertisementService.GetList(new AdvertisementQuery() { OrganisationId = organisation.Data.OrganisationId,Complated=true }, paginationQuery);
            if (result != null)
            {
                return Ok(result.Data);
            }
            return BadRequest(Messages.Error);
        }

        //tamam
        [Authorize(Policy = "OrganisationOnly")]
        [HttpGet("GetOrganisationProfileDetail")]
        public ActionResult GetOrganisationProfileDetail()
        {
            var userID = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var organisation = _organisationService.GetOrganisation(Convert.ToInt32(userID));
            if (organisation.Data == null)
            {
                return BadRequest("STK bulunumadı!");
            }
            var result = _organisationService.GetOrganisationProfileDetail(organisation.Data.OrganisationId);
            if (result != null)
            {
                return Ok(result.Data);
            }
            return BadRequest(Messages.Error);
        }

    }
}