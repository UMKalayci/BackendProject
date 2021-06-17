using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
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
            var result = _adversimentService.GetList(advertisementQuery,paginationQuery);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpGet("AddAdvertisement")]
        public ActionResult AddAdvertisement([FromQuery] AdvertisementDto advertisementDto)
        {
            var result = _adversimentService.Add(advertisementDto);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }
    }
}