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
    public class OrganisationController : ControllerBase
    {
        private IOrganisationService _organisationService;
        private IAuthService _authService;
        public OrganisationController(IOrganisationService organisationService, IAuthService authService)
        {
            _organisationService = organisationService;
            _authService = authService;
        }
        [HttpPost("register")]
        public ActionResult Register(OrganisationForRegisterDto organisationForRegisterDto)
        {
            var userExists = _organisationService.UserExists(organisationForRegisterDto.Email);
            if (!userExists.Success)
            {
                return BadRequest(userExists.Message);
            }

            var registerResult = _organisationService.Register(organisationForRegisterDto, organisationForRegisterDto.Password);
            if (registerResult.Success)
            {
                var result = _authService.CreateAccessToken(registerResult.Data.User);

                if (result.Success)
                {
                    //HttpContext.Session.SetInt32(SessionKeys.SessionKeyUserId, registerResult.Data.User.Id);
                    //HttpContext.Session.SetInt32(SessionKeys.SessionKeyOrganisationId, registerResult.Data.OrganisationId);
                    return Ok(result.Data);
                }
                return BadRequest(result.Message);
            }

            return BadRequest(registerResult.Message);
        }
    }
}