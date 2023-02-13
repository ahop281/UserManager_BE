using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using UserManager.API.Models.Request;
using UserManager.API.Services;

namespace UserManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private IConfiguration _config;
        private IAuthService authService;

        public AuthController(IConfiguration config, IAuthService authService)
        {
            _config = config;
            this.authService = authService;
        }

        /// <summary>
        /// Sign Up New User
        /// </summary>
        /// <returns>The registered user.</returns>
        /// <response code="200">Returns the registered user.</response>
        /// <response code="200">Returns the registered user.</response>
        /// <response code="500">Internal Server Error</response>  
        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpRequest request)
        {
            try
            {
                var result = await authService.SignUp(request);
                return Ok(result);
            }   
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Sign In
        /// </summary>
        /// <returns>The Logged In Token.</returns>
        /// <response code="200">Returns the logged in token.</response>
        /// <response code="200">Returns the registered user.</response>
        /// <response code="500">Internal Server Error</response>  
        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInRequest request)
        {
            try
            {
                var result = await authService.SignIn(request);
                return Ok(result);
            }   
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
