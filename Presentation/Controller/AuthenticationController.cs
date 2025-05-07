using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.IdentityDtos;

namespace Presentation.Controller
{
    public class AuthenticationController(IServiceManager serviceManager): ApiController
    {
        [HttpPost("Login")]
        public async Task<ActionResult<UserResultDto>> Login(LoginDto loginDto)
            => Ok(await serviceManager.AuthenticationService.LoginAsync(loginDto));

        [HttpPost("Register")]
        public async Task<ActionResult<UserResultDto>> Register(RegisterDto registerDto)
            => Ok(await serviceManager.AuthenticationService.RegisterAsync(registerDto));
    }
}
