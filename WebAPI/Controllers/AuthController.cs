﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Constants;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController:Controller
    {
        private IAuthService _authService;
        private IVolunteerService _volunteerService;

        public AuthController(IAuthService authService, IVolunteerService volunteerService)
        {
            _authService = authService;
            _volunteerService = volunteerService;
        }

        [HttpPost("login")]
        public ActionResult Login(UserForLoginDto userForLoginDto)
        {
            var userToLogin = _authService.Login(userForLoginDto);
            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin.Message);
            }

            var result = _authService.CreateAccessToken(userToLogin.Data);
            if (result.Success)
            {
                HttpContext.Session.SetInt32(SessionKeys.SessionKeyUserId, userToLogin.Data.Id);
                HttpContext.Session.SetInt32(SessionKeys.SessionType, userForLoginDto.Type);
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
