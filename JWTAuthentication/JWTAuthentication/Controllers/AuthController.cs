using JWTAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace JWTAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("GetUserDetails")]
        [Authorize]
        public IActionResult GetUserDetails()
        {
            var currentUser = GetCurrentUser();

            return Ok(currentUser);
        }

        [HttpGet("GetAdminDetails")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAdminDetails()
        {
            var currentUser = GetCurrentUser();

            return Ok(currentUser);
        }

        [HttpGet("GetClientDetails")]
        [Authorize(Roles = "Client")]
        public IActionResult GetClientDetails()
        {
            var currentUser = GetCurrentUser();

            return Ok(currentUser);
        }

        private UserModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var claims = identity.Claims;

                return new UserModel()
                {
                    Username = claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                    Email = claims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    Surname = claims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                    GivenName = claims.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
                    Role = claims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
                };
            }

            return null;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        [Produces("application/json")]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var user = Authenticate(userLogin);

            if (user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }

            return NotFound("User not found!");
        }

        private string Generate(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.GivenName),
                new Claim(ClaimTypes.Surname, user.Surname),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                _config["JWT:Issuer"],
                _config["JWT:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel Authenticate(UserLogin userLogin)
        {
            var user = UserContants.Users.FirstOrDefault(x => x.Username == userLogin.Username && x.Password == userLogin.Password);
            return user;
        }
    }
}
