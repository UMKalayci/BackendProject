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
            var result = _adversimentService.GetList(advertisementQuery, paginationQuery);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [Authorize(Policy = "OrganisationOnly")]
        [HttpGet("AddAdvertisement")]
        public ActionResult AddAdvertisement([FromQuery] AdvertisementDto advertisementDto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var userClaim = claim
                .Where(x => x.Type == ClaimTypes.Role)
                .FirstOrDefault().Value;
            if (userClaim == "STK")
            {
                var result = _adversimentService.Add(advertisementDto);
                if (result.Success)
                {
                    return Ok(result.Data);
                }
                return BadRequest(result.Message);
            }
            else
            {
                return BadRequest(Messages.AuthorizationDenied);
            }
        }
    }
}