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

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteersController : ControllerBase
    {
        private IVolunteerService _volunteerService;
        private IAuthService _authService;

        public VolunteersController(IVolunteerService VolunteerService,IAuthService authService)
        {
            _volunteerService = VolunteerService;
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
            var result = _authService.CreateAccessToken(registerResult.Data.User);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }
    }
}