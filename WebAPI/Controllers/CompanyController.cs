using Business.Abstract;
using Entities.Dtos;
using Entities.QueryModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

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
        [HttpGet("GetVolunteerList")]
        public ActionResult GetVolunteerList()
        {
            var userID = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var company = _companyService.GetCompany(Convert.ToInt32(userID));
            if (company.Data == null)
            {
                return BadRequest("Şirket bulunumadı!");
            }
            var result = _companyService.GetCompanyVolunteerList(company.Data.CompanyId);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [Authorize]
        [HttpGet("GetCompanyComboList")]
        public ActionResult GetCompanyComboList()
        {
            var result = _companyService.GetComboList();
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }
        [Authorize(Policy = "CompanyOnly")]
        [HttpGet("GetProfilDetail")]
        public ActionResult GetProfilDetail()
        {
            var userID = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var company = _companyService.GetCompany(Convert.ToInt32(userID));
            if (company.Data == null)
            {
                return BadRequest("Şirket bulunumadı!");
            }
            var result = _companyService.GetProfilDetail(company.Data.CompanyId);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }
        [Authorize(Policy = "CompanyOnly")]
        [HttpGet("GetDashboardDetail")]
        public ActionResult GetDashboardDetail()
        {
            var userID = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var company = _companyService.GetCompany(Convert.ToInt32(userID));
            if (company.Data == null)
            {
                return BadRequest("Şirket bulunumadı!");
            }
            var result = _companyService.GetCompanyDashboard(company.Data.CompanyId);
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
