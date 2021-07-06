using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
            }
            else if (userForLoginDto.Type == 2)
            {
                var organisation = _organisationService.GetOrganisation(userToLogin.Data.Id);
                if (organisation.Data != null && organisation.Data.Status == false)
                {
                    return BadRequest("Hesabınızın onaylanmasını bekleyiniz!");
                }
                if (!organisation.Success)
                {
                    return BadRequest(organisation.Message);
                }
            }
            var result = _authService.CreateAccessToken(userToLogin.Data);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }
        [HttpGet("ConfirmEmail")]
        public IActionResult ConfirmEmail([FromQuery] string token, [FromQuery] string email)
        {
            if(ValidateJwtToken(token)!=email)
                return View("Error");
            var user =  _authService.FindByEmail(email);
            if (user == null)
                return View("Error");

            var result = _authService.ConfirmEmail(email);
            return View(result.Success ? "ConfirmEmail" : "Error");
        }

       public string ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("mysecretkeymysecretkey");
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = "www.egonullu.com",
                    ValidAudience = "www.egonullu.com",
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var email = jwtToken.Claims.First(x => x.Type == "email").Value.ToString();

                // return account id from JWT token if validation successful
                return email;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
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
