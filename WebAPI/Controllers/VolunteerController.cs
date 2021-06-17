using System;
using System.Collections.Generic;
using System.Linq;
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
                    HttpContext.Session.SetInt32(SessionKeys.SessionType, 1);
                    HttpContext.Session.SetInt32(SessionKeys.SessionKeyUserId, registerResult.Data.User.Id);
                    HttpContext.Session.SetInt32(SessionKeys.SessionKeyVolunteerId, registerResult.Data.VolunteerId);
                    return Ok(result.Data);
                }
                return BadRequest(result.Message);
            }

            return BadRequest(registerResult.Message);
        }

        [HttpPost("EnrollAdvertisement")]
        public ActionResult EnrollAdvertisement(AdvertisementVolunteerDto advertisementVolunteerDto)
        {
            var result = _volunteerService.EnrollAdvertisement(advertisementVolunteerDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return BadRequest(result.Message);
        }

    }
}