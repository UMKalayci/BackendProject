using Business.Abstract;
using Entities.Dtos;
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
        private IAuthService _authService;
        public OrganisationController(IOrganisationService organisationService, IAuthService authService)
        {
            _organisationService = organisationService;
            _authService = authService;
        }
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
    }
}