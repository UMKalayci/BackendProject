using Business.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebAPI.Constants;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private IAuthService _authService;
        private IVolunteerService _volunteerService;
        private IOrganisationService _organisationService;

        public AuthController(IAuthService authService, IVolunteerService volunteerService, IOrganisationService organisationService)
        {
            _authService = authService;
            _volunteerService = volunteerService;
            _organisationService = organisationService;
        }

        [HttpPost("login")]
        public ActionResult Login(UserForLoginDto userForLoginDto)
        {
            var userToLogin = _authService.Login(userForLoginDto);
            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin.Message);
            }
            if (userForLoginDto.Type == 1)
            {
                var volunteer = _volunteerService.GetVolunteer(userToLogin.Data.Id);
                if (!volunteer.Success)
                {
                    return BadRequest(volunteer.Message);
                }
                else
                {
                    var bytes = Encoding.UTF8.GetBytes(volunteer.Data.VolunteerId.ToString());
                    HttpContext.Session.Set(SessionKeys.SessionKeyVolunteerId, bytes);
                }
            }
            else if (userForLoginDto.Type == 2)
            {
                var organisation = _organisationService.GetOrganisation(userToLogin.Data.Id);
                if (!organisation.Success)
                {
                    return BadRequest(organisation.Message);
                }
                else
                {

                    var bytes = Encoding.UTF8.GetBytes(organisation.Data.OrganisationId.ToString());
                    HttpContext.Session.Set(SessionKeys.SessionKeyOrganisationId, bytes);
                }
            }
            var result = _authService.CreateAccessToken(userToLogin.Data);
            if (result.Success)
            {
                var bytes = Encoding.UTF8.GetBytes(userToLogin.Data.Id.ToString());
                HttpContext.Session.Set(SessionKeys.SessionKeyUserId, bytes);

                var bytes2 = Encoding.UTF8.GetBytes(userForLoginDto.Type.ToString());
                HttpContext.Session.Set(SessionKeys.SessionType, bytes2);
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpPost("mailauth")]
        public ActionResult MailAuth(UserMailAuthDto userMailAuthDto)
        {
            var userExists = _authService.UserExists(userMailAuthDto.Email);
            if (!userExists.Success)
            {
                return BadRequest(userExists.Message);
            }

            var registerResult = _authService.MailConfirmation(userMailAuthDto);
            if (registerResult.Success)
            {
                return Ok(registerResult.Data);
            }

            return BadRequest(registerResult.Message);
        }

        //[HttpPost("register")]
        //public ActionResult Register(UserForRegisterDto userForRegisterDto)
        //{
        //    var userExists = _authService.UserExists(userForRegisterDto.Email);
        //    if (!userExists.Success)
        //    {
        //        return BadRequest(userExists.Message);
        //    }

        //    var registerResult = _authService.Register(userForRegisterDto,userForRegisterDto.Password);
        //    var result = _authService.CreateAccessToken(registerResult.Data);
        //    if (result.Success)
        //    {
        //        return Ok(result.Data);
        //    }

        //    return BadRequest(result.Message);
        //}
    }
}
