using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Extensions;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Constants;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteerController : ControllerBase
    {
        private IVolunteerService _volunteerService;
        private IAuthService _authService;

        public VolunteerController(IVolunteerService volunteerService, IAuthService authService)
        {
            _volunteerService = volunteerService;
            _authService = authService;
        }

        [HttpPost("register")]
        public ActionResult Register(VolunteerForRegisterDto volunteerForRegisterDto)
        {
            var userExists = _volunteerService.UserExists(volunteerForRegisterDto.Email);
            if (!userExists.Success)
            {
                return BadRequest(userExists.Message);
            }

            var registerResult = _volunteerService.Register(volunteerForRegisterDto, volunteerForRegisterDto.Password);
            if (registerResult.Success)
            {
                var result = _authService.CreateAccessToken(registerResult.Data.User);

                if (result.Success)
                {
                    //HttpContext.Session.SetInt32(SessionKeys.SessionKeyUserId, registerResult.Data.User.Id);
                    //HttpContext.Session.SetInt32(SessionKeys.SessionKeyVolunteerId, registerResult.Data.VolunteerId);
                    return Ok(result.Data);
                }
                return BadRequest(result.Message);
            }

            return BadRequest(registerResult.Message);
        }

        [Authorize(Policy = "VolunteerOnly")]
        [HttpPost("EnrollAdvertisement")]
        public ActionResult EnrollAdvertisement(AdvertisementVolunteerDto advertisementVolunteerDto)
        {
            var userID = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var volunteer = _volunteerService.GetVolunteer(Convert.ToInt32(userID));
            if (volunteer.Data == null)
            {
                return BadRequest("STK bulunumadı!");
            }
            advertisementVolunteerDto.VolunteerId = volunteer.Data.VolunteerId;
            var result = _volunteerService.EnrollAdvertisement(advertisementVolunteerDto);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        [Authorize(Policy = "VolunteerOnly")]
        [HttpPost("ComplatedAdvertisement")]
        public ActionResult ComplatedAdvertisement(VolunteerAdvertisementComplatedDto volunteerAdvertisementComplatedDto)
        {
            var userID = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var volunteer = _volunteerService.GetVolunteer(Convert.ToInt32(userID));
            if (volunteer.Data == null)
            {
                return BadRequest("STK bulunumadı!");
            }
            volunteerAdvertisementComplatedDto.VolunteerId = volunteer.Data.VolunteerId;

            var result = _volunteerService.ComplatedAdvertisement(volunteerAdvertisementComplatedDto);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        [Authorize(Policy = "VolunteerOnly")]
        [HttpGet("GetVolunteerProfile")]
        public ActionResult GetVolunteerProfile()
        {
            var userID = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;

            var result = _volunteerService.GetVolunterProfile(Convert.ToInt32(userID));
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }


    }
}