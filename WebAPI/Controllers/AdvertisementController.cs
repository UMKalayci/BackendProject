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
        public AdvertisementController(IAdvertisementService adversimentService)
        {
            _adversimentService = adversimentService;
        }

        [HttpGet("GetList")]
        public ActionResult GetList([FromQuery] AdvertisementQuery advertisementQuery, [FromQuery] PaginationQuery paginationQuery)
        {
            if (HttpContext.Session.GetInt32(SessionKeys.SessionKeyVolunteerId) != null)
                advertisementQuery.VolunteerId = HttpContext.Session.GetInt32(SessionKeys.SessionKeyVolunteerId).Value;
            var result = _adversimentService.GetList(advertisementQuery, paginationQuery);
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
            if (HttpContext.Session.GetInt32(SessionKeys.SessionKeyOrganisationId) == null)
                return BadRequest("STK bulunumadı!");
            var oganisationId = HttpContext.Session.GetInt32(SessionKeys.SessionKeyOrganisationId).Value;

            advertisementDto.OrganisationId = oganisationId;
            var result = _adversimentService.Add(advertisementDto);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }
    }
}