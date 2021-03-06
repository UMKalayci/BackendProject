using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Entities.Concrete;
using Core.Extensions;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private IEmailHelper _emailHelper;


        public VolunteerController(IVolunteerService volunteerService, IEmailHelper emailHelper, IAuthService authService)
        {
            _volunteerService = volunteerService;
            _authService = authService;
            _emailHelper = emailHelper;

        }
        //tamam
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

                var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { result.Data.Token, email = registerResult.Data.User.Email }, Request.Scheme);

                var emailResponse = _emailHelper.MailConfirmation(registerResult.Data.User.Email, confirmationLink);

                if (emailResponse.Success)
                {
                    return Ok();
                }
                return BadRequest(emailResponse.Message);
            }

            return BadRequest(registerResult.Message);
        }

        //tamam
        [Authorize(Policy = "VolunteerOnly")]
        [HttpPost("update")]
        public ActionResult Update(VolunteerForRegisterDto volunteerForRegisterDto)
        {
            var userExists = _volunteerService.UserExists(volunteerForRegisterDto.Email);
            if (userExists.Success)
            {
                return BadRequest(userExists.Message);
            }

            var updateResult = _volunteerService.Update(volunteerForRegisterDto, volunteerForRegisterDto.Password);
            if (updateResult.Success)
            {
                return Ok();
            }

            return BadRequest(updateResult.Message);
        }

        //tamam
        [Authorize(Policy = "VolunteerOnly")]
        [HttpPost("EnrollAdvertisement")]
        public ActionResult EnrollAdvertisement(AdvertisementVolunteerDto advertisementVolunteerDto)
        {
            var userID = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var volunteer = _volunteerService.GetVolunteer(Convert.ToInt32(userID));
            if (volunteer.Data == null)
            {
                return BadRequest("Gönüllü bulunumadı!");
            }
            advertisementVolunteerDto.VolunteerId = volunteer.Data.VolunteerId;
            var result = _volunteerService.EnrollAdvertisement(advertisementVolunteerDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        //tamam
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

        //tamam
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

        //tamam
        [Authorize(Policy = "VolunteerOnly")]
        [HttpGet("GetActiveAdvertisementList")]
        public ActionResult GetActiveAdvertisementList()
        {
            var userID = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var volunteer = _volunteerService.GetVolunteer(Convert.ToInt32(userID));
            if (volunteer.Data == null)
            {
                return BadRequest("Gönüllü bulunumadı!");
            }
            var result = _volunteerService.GetActiveAdvertisementList(volunteer.Data.VolunteerId);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        //tamam
        [Authorize(Policy = "VolunteerOnly")]
        [HttpGet("GetComplatedAdvertisementList")]
        public ActionResult GetComplatedAdvertisementList()
        {
            var userID = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var volunteer = _volunteerService.GetVolunteer(Convert.ToInt32(userID));
            if (volunteer.Data == null)
            {
                return BadRequest("Gönüllü bulunumadı!");
            }
            var result = _volunteerService.GetComplatedAdvertisementList(volunteer.Data.VolunteerId);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        //tamam
        [Authorize(Policy = "VolunteerOnly")]
        [HttpGet("GetVolunteerDashboard")]
        public ActionResult GetVolunteerDashboard()
        {
            var userID = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var organisation = _volunteerService.GetVolunteer(Convert.ToInt32(userID));
            if (organisation.Data == null)
            {
                return BadRequest("STK bulunumadı!");
            }
            var result = _volunteerService.GeVolunteerDashboard(organisation.Data.VolunteerId);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }
    }
}