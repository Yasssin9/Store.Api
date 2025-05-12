﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.IdentityDtos;

namespace Presentation.Controller
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthenticationController(IServiceManager serviceManager): ApiController
    {
        [HttpPost]
        public async Task<ActionResult<UserResultDto>> Login(LoginDto loginDto)
            => Ok(await serviceManager.AuthenticationService.LoginAsync(loginDto));

        [HttpPost]
        public async Task<ActionResult<UserResultDto>> Register(RegisterDto registerDto)
          => Ok(await serviceManager.AuthenticationService.RegisterAsync(registerDto));

        [HttpGet]     
        public async Task<ActionResult<bool>> IsEmailExits(string email)
            => Ok(await serviceManager.AuthenticationService.IsEmailExist(email));

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserResultDto>> GetCurrentUser()
        {
            var email=User.FindFirst(ClaimTypes.Email).Value;
            var result= await serviceManager.AuthenticationService.GetUserByEmailAsync(email);
            return Ok(result);
        }


        [HttpGet]
        [Authorize]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;
            var result = await serviceManager.AuthenticationService.GetUserAddressAsync(email);
            return Ok(result);
        }


        [HttpPut] //partially update
        [Authorize]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto addressDto)
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;
            var result = await serviceManager.AuthenticationService.UpdateUserAddressAsync(email, addressDto);
            return Ok(result);
        }
    }
}
