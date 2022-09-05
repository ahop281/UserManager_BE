using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using UserManager.API.Models.DTO;
using UserManager.API.Services;

namespace UserManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetAllUsers")]
        [ProducesResponseType(typeof(List<UserDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await _userService.GetAllUsers();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet]
        [Route("GetUser")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUser(GetUserRequest request)
        {
            try
            {
                if (!ValidateGetUser(request))
                {
                    return BadRequest(ModelState);
                }

                var result = await _userService.GetUser(request.Id);
                if (result == null)
                    return NotFound("User not found");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        [Route("AddUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddUser(AddUserRequest request)
        {
            try
            {
                if (!ValidateAddUser(request))
                {
                    return BadRequest(ModelState);
                }

                var result = await _userService.AddUser(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        [Route("UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser(UpdateUserRequest request)
        {
            try
            {
                if (!ValidateUpdateUser(request))
                {
                    return BadRequest(ModelState);
                }

                var result = await _userService.UpdateUser(request);
                if (result == null)
                    return NotFound("User not found");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        [Route("DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser(DeleteUserRequest request)
        {
            try
            {
                if (!ValidateDeleteUser(request))
                {
                    return BadRequest(ModelState);
                }

                var result = await _userService.DeleteUser(request.Id);
                if (result == null)
                    return NotFound("User not found");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        #region Private Methods

        private static bool IsValidEmail(string email)
        {
            var valid = true;

            try
            {
                var emailAddress = new MailAddress(email);
            }
            catch
            {
                valid = false;
            }

            return valid;
        }

        private bool ValidateGetUser(GetUserRequest request)
        {
            if (request == null)
            {
                ModelState.AddModelError(nameof(GetUserRequest), $"Request is required!");
                return false;
            }

            if (request.Id == Guid.Empty)
            {
                ModelState.AddModelError(nameof(request.Id), $"{request.Id} cannot be null!");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }

        private bool ValidateAddUser(AddUserRequest request)
        {
            if (request == null)
            {
                ModelState.AddModelError(nameof(GetUserRequest), $"Request is required!");
                return false;
            }

            if (string.IsNullOrEmpty(request.Name))
            {
                ModelState.AddModelError(nameof(request.Name), $"{request.Name} cannot be null or empty!");
            }

            if (string.IsNullOrEmpty(request.Email))
            {
                ModelState.AddModelError(nameof(request.Email), $"{request.Email} cannot be null or empty!");
            }
            else if (!IsValidEmail(request.Email))
            {
                ModelState.AddModelError(nameof(request.Email), $"{request.Email} is invalid email!");
            }

            if (string.IsNullOrEmpty(request.Address))
            {
                ModelState.AddModelError(nameof(request.Address), $"{request.Address} cannot be null or empty!");
            }

            if (request.Dob == DateTime.MinValue)
            {
                ModelState.AddModelError(nameof(request.Dob), $"{request.Dob} cannot be null!");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }

        private bool ValidateUpdateUser(UpdateUserRequest request)
        {
            if (request == null)
            {
                ModelState.AddModelError(nameof(GetUserRequest), $"Request is required!");
                return false;
            }

            if (request.Id == Guid.Empty)
            {
                ModelState.AddModelError(nameof(request.Id), $"{request.Id} cannot be null!");
            }

            if (string.IsNullOrEmpty(request.Name))
            {
                ModelState.AddModelError(nameof(request.Name), $"{request.Name} cannot be null or empty!");
            }

            if (string.IsNullOrEmpty(request.Email))
            {
                ModelState.AddModelError(nameof(request.Email), $"{request.Email} cannot be null or empty!");
            }
            else if (!IsValidEmail(request.Email))
            {
                ModelState.AddModelError(nameof(request.Email), $"{request.Email} is invalid email!");
            }

            if (string.IsNullOrEmpty(request.Address))
            {
                ModelState.AddModelError(nameof(request.Address), $"{request.Address} cannot be null or empty!");
            }

            if (request.Dob == DateTime.MinValue)
            {
                ModelState.AddModelError(nameof(request.Dob), $"{request.Dob} cannot be null!");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }

        private bool ValidateDeleteUser(DeleteUserRequest request)
        {
            if (request == null)
            {
                ModelState.AddModelError(nameof(GetUserRequest), $"Request is required!");
                return false;
            }

            if (request.Id == Guid.Empty)
            {
                ModelState.AddModelError(nameof(request.Id), $"{request.Id} cannot be null!");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
