using AutoMapper;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserManager.API.Models.DTO;
using UserManager.API.Models.Entities;
using UserManager.API.Models.Request;
using UserManager.API.Models.Response;
using UserManager.API.Repositories;

namespace UserManager.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public readonly int EXPIRESIN = 3600;

        public AuthService(IGenericRepository<User> context, IMapper mapper, IConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
        }

        public async Task<SignUpResponse> SignUp(SignUpRequest request)
        {
            try
            {
                var existedUser = await _context.GetUserByAttribute("Username", request.Username);

                if (existedUser != null)
                    throw new Exception("Username already in used!");

                existedUser = await _context.GetUserByAttribute("Email", request.Email);

                if (existedUser != null)
                    throw new Exception("Email already in used!");

                var userEntity = _mapper.Map<User>(request);
                userEntity.Id = Guid.NewGuid();
                userEntity.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

                var result = await _context.AddUser(userEntity);
                return _mapper.Map<SignUpResponse>(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        public async Task<SignInResponse> SignIn(SignInRequest request)
        {
            try
            {
                var existedUser = await _context.GetUserByAttribute("Username", request.Username);

                if (existedUser == null)
                    throw new Exception("Username does not exist!");

                var verified = BCrypt.Net.BCrypt.Verify(request.Password, existedUser.Password);
                if (!verified)
                    throw new Exception("Password is not correct!");

                var token = Generate(existedUser);

                return new SignInResponse()
                {
                    AccessToken = token,
                    ExpiresIn = EXPIRESIN
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private string Generate(User existedUser)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, existedUser.Username),
                new Claim(ClaimTypes.Name, existedUser.Name),
                new Claim(ClaimTypes.Email, existedUser.Email)
            };

            var token = new JwtSecurityToken(
                _config["JWT:Issuer"],
                _config["JWT:Audience"],
                claims,
                expires: DateTime.Now.AddSeconds(EXPIRESIN),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
