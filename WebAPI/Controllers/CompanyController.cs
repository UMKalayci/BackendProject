using Business.Abstract;
using Entities.Dtos;
using Entities.QueryModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : Controller
    {
        private ICompanyService _companyService;
        private IAuthService _authService;
        public CompanyController(ICompanyService companyService, IAuthService authService)
        {
            _companyService = companyService;
            _authService = authService;
        }

        [Authorize(Policy = "CompanyOnly")]
        [HttpGet("GetList")]
        public ActionResult GetList()
        {
            var result = _companyService.GetList();
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }


        [HttpPost("register")]
        public ActionResult Register(CompanyForRegisterDto companyForRegisterDto)
        {
            var userExists = _companyService.UserExists(companyForRegisterDto.Email);
            if (!userExists.Success)
            {
                return BadRequest(userExists.Message);
            }

            var registerResult = _companyService.Register(companyForRegisterDto, companyForRegisterDto.Password);
            if (registerResult.Success)
            {
                var result = _authService.CreateAccessToken(registerResult.Data.User);

                if (result.Success)
                {
                    return Ok(result.Data);
                }
                return BadRequest(result.Message);
            }

            return BadRequest(registerResult.Message);
        }

    }
}
