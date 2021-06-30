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
    public class ConstController : ControllerBase
    {
        private ICategoryService _categoryService;
        private IPurposeService _purposeService;

        public ConstController(ICategoryService categoryService, IPurposeService purposeService)
        {
            _categoryService = categoryService;
            _purposeService = purposeService;
        }

        [HttpGet("GetCategoryList")]
        public ActionResult GetCategoryList()
        {  var result = _categoryService.GetList();
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }


        [HttpGet("GetPurposeList")]
        public ActionResult GetPurposeList()
        {
            var result = _purposeService.GetList();
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

    }
}