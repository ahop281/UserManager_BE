using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using UserManager.API.Controllers;
using UserManager.API.Models.DTO;
using UserManager.API.Services;
using Xunit;

namespace UserManager.API.Test
{
    public class UserControllerTest
    {
        Mock<IUserService> _mockUserService;
        private UserController _target;
        private Mock<ILogger<UserController>> _logger;
        private List<UserDTO> _list;

        private void SetUp()
        {
            _mockUserService = new Mock<IUserService>();
            _logger = new Mock<ILogger<UserController>>();
            _target = new UserController(_mockUserService.Object, _logger.Object);
            _list = new List<UserDTO>()
            {
                new UserDTO()
                {
                    Id = Guid.NewGuid(),
                    Name = "user 1",
                    Email = "email1@email.com",
                    Address = "address 1",
                    Dob = DateTime.Now
                },
                new UserDTO()
                {
                    Id = Guid.NewGuid(),
                    Name = "user 2",
                    Email = "email2@email.com",
                    Address = "address 2",
                    Dob = DateTime.Now
                }
            };
        }

        #region GetAllUsers
        [Fact]
        public async void GetAllUsers_ReturnsOkResult()
        {
            SetUp();

            _mockUserService.CallBase = true;

            _mockUserService.Setup(x => x.GetAllUsers())
                                     .ReturnsAsync(_list);

            var actual = await _target.GetAllUsers();

            Assert.IsType<OkObjectResult>(actual);
        }

        [Fact]
        public async void GetAllUsers_ReturnsInternalServerError()
        {
            SetUp();

            _mockUserService.CallBase = true;

            _mockUserService.Setup(x => x.GetAllUsers())
                                     .Throws<Exception>();

            var expected = (int)HttpStatusCode.InternalServerError;
            var actual = await _target.GetAllUsers();
            var statusCodeResult = (IStatusCodeActionResult)actual;

            Assert.Equal(statusCodeResult.StatusCode, expected);
        }
        #endregion

        #region GetUser
        [Fact]
        public async void GetUser_ReturnsOkResult()
        {
            SetUp();
            var index = 0;
            var request = new GetUserRequest()
            {
                Id = _list[index].Id
            };

            _mockUserService.CallBase = true;

            _mockUserService.Setup(x => x.GetUser(request.Id))
                                     .ReturnsAsync(_list[index]);

            var actual = await _target.GetUser(request);

            Assert.IsType<OkObjectResult>(actual);
        }

        [Fact]
        public async void GetUser_ReturnsUserNotFound()
        {
            SetUp();
            var request = new GetUserRequest()
            {
                Id = Guid.NewGuid()
            };

            _mockUserService.CallBase = true;

            _mockUserService.Setup(x => x.GetUser(request.Id))
                                     .ReturnsAsync((UserDTO)null);

            var expected = (int)HttpStatusCode.NotFound;
            var actual = await _target.GetUser(request);
            var statusCodeResult = (IStatusCodeActionResult)actual;

            Assert.Equal(statusCodeResult.StatusCode, expected);
        }

        [Fact]
        public async void GetUser_ReturnsInternalServerError()
        {
            SetUp();
            var request = new GetUserRequest()
            {
                Id = Guid.NewGuid()
            };

            _mockUserService.CallBase = true;

            _mockUserService.Setup(x => x.GetUser(request.Id))
                                     .Throws<Exception>();

            var expected = (int)HttpStatusCode.InternalServerError;
            var actual = await _target.GetUser(request);
            var statusCodeResult = (IStatusCodeActionResult)actual;

            Assert.Equal(statusCodeResult.StatusCode, expected);
        }
        #endregion

        #region AddUser
        [Fact]
        public async void AddUser_ReturnsOkResult()
        {
            SetUp();
            var request = new AddUserRequest()
            {
                Name = "New User",
                Email = "user@email.com",
                Address = "user address",
                Dob = DateTime.Now
            };

            var response = new UserDTO()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Address = request.Address,
                Dob = request.Dob
            };

            _mockUserService.CallBase = true;

            _mockUserService.Setup(x => x.AddUser(request))
                .ReturnsAsync(response);

            var actual = await _target.AddUser(request);

            Assert.IsType<OkObjectResult>(actual);
        }

        [Fact]
        public async void AddUser_ReturnsInternalServerError()
        {
            SetUp();
            var request = new AddUserRequest()
            {
                Name = "New User",
                Email = "user@email.com",
                Address = "user address",
                Dob = DateTime.Now
            };

            _mockUserService.CallBase = true;

            _mockUserService.Setup(x => x.AddUser(request))
                                     .Throws<Exception>();

            var expected = (int)HttpStatusCode.InternalServerError;
            var actual = await _target.AddUser(request);
            var statusCodeResult = (IStatusCodeActionResult)actual;

            Assert.Equal(statusCodeResult.StatusCode, expected);
        }
        #endregion

        #region UpdateUser
        [Fact]
        public async void UpdateUser_ReturnsOkResult()
        {
            SetUp();
            var request = new UpdateUserRequest()
            {
                Id = Guid.NewGuid(),
                Name = "New User",
                Email = "user@email.com",
                Address = "user address",
                Dob = DateTime.Now
            };

            var response = new UserDTO()
            {
                Id = request.Id,
                Name = request.Name,
                Email = request.Email,
                Address = request.Address,
                Dob = request.Dob
            };

            _mockUserService.CallBase = true;

            _mockUserService.Setup(x => x.UpdateUser(request))
                                     .ReturnsAsync(response);

            var actual = await _target.UpdateUser(request);

            Assert.IsType<OkObjectResult>(actual);
        }

        [Fact]
        public async void UpdateUser_ReturnsUserNotFound()
        {
            SetUp();
            var user = new UpdateUserRequest()
            {
                Id = Guid.NewGuid(),
                Name = "New User",
                Email = "user@email.com",
                Address = "user address",
                Dob = DateTime.Now
            };

            _mockUserService.CallBase = true;

            _mockUserService.Setup(x => x.UpdateUser(user))
                                     .ReturnsAsync((UserDTO)null);

            var expected = (int)HttpStatusCode.NotFound;
            var actual = await _target.UpdateUser(user);
            var statusCodeResult = (IStatusCodeActionResult)actual;

            Assert.Equal(statusCodeResult.StatusCode, expected);
        }

        [Fact]
        public async void UpdateUser_ReturnsInternalServerError()
        {
            SetUp();
            var user = new UpdateUserRequest()
            {
                Id = Guid.NewGuid(),
                Name = "New User",
                Email = "user@email.com",
                Address = "user address",
                Dob = DateTime.Now
            };

            _mockUserService.CallBase = true;

            _mockUserService.Setup(x => x.UpdateUser(user))
                                     .Throws<Exception>();

            var expected = (int)HttpStatusCode.InternalServerError;
            var actual = await _target.UpdateUser(user);
            var statusCodeResult = (IStatusCodeActionResult)actual;

            Assert.Equal(statusCodeResult.StatusCode, expected);
        }
        #endregion

        #region DeleteUser
        [Fact]
        public async void DeleteUser_ReturnsOkResult()
        {
            SetUp();
            var index = 0;
            var request = new DeleteUserRequest()
            {
                Id = _list[index].Id
            };

            _mockUserService.CallBase = true;

            _mockUserService.Setup(x => x.DeleteUser(request.Id))
                                     .ReturnsAsync(_list[index]);

            var actual = await _target.DeleteUser(request);

            Assert.IsType<OkObjectResult>(actual);
        }

        [Fact]
        public async void DeleteUser_ReturnsUserNotFound()
        {
            SetUp();
            var request = new DeleteUserRequest()
            {
                Id = Guid.NewGuid()
            };

            _mockUserService.CallBase = true;

            _mockUserService.Setup(x => x.DeleteUser(request.Id))
                                     .ReturnsAsync((UserDTO)null);

            var expected = (int)HttpStatusCode.NotFound;
            var actual = await _target.DeleteUser(request);
            var statusCodeResult = (IStatusCodeActionResult)actual;

            Assert.Equal(statusCodeResult.StatusCode, expected);
        }

        [Fact]
        public async void DeleteUser_ReturnsInternalServerError()
        {
            SetUp();
            var request = new DeleteUserRequest()
            {
                Id = Guid.NewGuid()
            };

            _mockUserService.CallBase = true;

            _mockUserService.Setup(x => x.DeleteUser(request.Id))
                                     .Throws<Exception>();

            var expected = (int)HttpStatusCode.InternalServerError;
            var actual = await _target.DeleteUser(request);
            var statusCodeResult = (IStatusCodeActionResult)actual;

            Assert.Equal(statusCodeResult.StatusCode, expected);
        }
        #endregion
    }
}
