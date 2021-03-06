using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Constants;
using Core.Extensions;
using Entities.Concrete;
using Entities.Dtos;
using Entities.QueryModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Constants;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdvertisementController : ControllerBase
    {
        private IAdvertisementService _adversimentService;
        private IOrganisationService _organisationService;
        private IVolunteerService _volunteerService;
        public AdvertisementController(IAdvertisementService adversimentService, IVolunteerService volunteerService, IOrganisationService organisationService)
        {
            _adversimentService = adversimentService;
            _volunteerService = volunteerService;
            _organisationService = organisationService;
        }

        [HttpGet("GetList")]
        public ActionResult GetList([FromQuery] AdvertisementQuery advertisementQuery, [FromQuery] PaginationQuery paginationQuery)
        {
            var userID = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var volunteer = _volunteerService.GetVolunteer(Convert.ToInt32(userID));
            if (volunteer.Data == null)
            {
                return BadRequest("STK bulunumadı!");
            }


            advertisementQuery.VolunteerId = volunteer.Data.VolunteerId;
            var result = _adversimentService.GetList(advertisementQuery, paginationQuery);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }
        [HttpGet("GetAdvertisementDetail")]
        public ActionResult GetAdvertisementDetail([FromQuery] int advertisementId)
        {
            var result = _adversimentService.GetAdvertisementDetail(advertisementId);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [Authorize(Policy = "OrganisationOnly")]
        [HttpPost("AddAdvertisement")]
        public ActionResult AddAdvertisement(AdvertisementDto advertisementDto)
        {
            var userID = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var organisation= _organisationService.GetOrganisation(Convert.ToInt32(userID));
            if (organisation.Data == null)
            {
                return BadRequest("STK bulunumadı!");
            }
            advertisementDto.OrganisationId = organisation.Data.OrganisationId;
            var result = _adversimentService.Add(advertisementDto);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [Authorize(Policy = "VolunteerOnly")]
        [HttpPost("AddComment")]
        public ActionResult AddComment(CommentDto comment)
        {
            var userID = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var volunteer = _volunteerService.GetVolunteer(Convert.ToInt32(userID));
            if (volunteer.Data == null)
            {
                return BadRequest("Gönüllü bulunumadı!");
            }
            comment.VolunteerId = volunteer.Data.VolunteerId;
            var result = _adversimentService.AddComment(comment);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

    }
}