using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using UserManager.API.Models.DTO;
using UserManager.API.Models.Entities;
using UserManager.API.Repositories;
using UserManager.API.Services;
using UserManager_BE.Profiles;
using Xunit;

namespace UserManager.API.Test
{
    public class UserServiceTest
    {
        private Mock<IGenericRepository<User>> _mockRepository;
        private IMapper _mapper;
        private UserService _target;
        private List<User> _list;

        private void SetUp()
        {
            _mockRepository = new Mock<IGenericRepository<User>>();

            var userProfile = new UserProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(userProfile));
            _mapper = new Mapper(configuration);

            _target = new UserService(_mockRepository.Object, _mapper);
            _list = new List<User>()
            {
                new User()
                {
                    Id = Guid.NewGuid(),
                    Name = "user 1",
                    Email = "email1@email.com",
                    Address = "address 1",
                    Dob = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Name = "user 2",
                    Email = "email2@email.com",
                    Address = "address 2",
                    Dob = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                }
            };
        }

        #region GetAllUsers
        [Fact]
        public async void GetAllUsers_ReturnsAllUser()
        {
            SetUp();

            _mockRepository.CallBase = true;

            _mockRepository.Setup(x => x.GetAllUsers())
                                     .ReturnsAsync(_list);

            var actual = await _target.GetAllUsers();

            Assert.IsType<List<UserDTO>>(actual);
        }

        [Fact]
        public void GetAllUsers_ThrowsException()
        {
            SetUp();

            _mockRepository.CallBase = true;

            _mockRepository.Setup(x => x.GetAllUsers())
                                     .Throws<Exception>();

            Assert.ThrowsAsync<Exception>(async () => await _target.GetAllUsers());
        }
        #endregion

        #region GetUser
        [Fact]
        public async void GetUser_ReturnsUser()
        {
            SetUp();
            var index = 0;

            _mockRepository.CallBase = true;

            _mockRepository.Setup(x => x.GetUser(_list[index].Id))
                                     .ReturnsAsync(_list[index]);

            var actual = await _target.GetUser(_list[index].Id);

            Assert.IsType<UserDTO>(actual);
        }

        [Fact]
        public async void GetUser_ReturnsNull()
        {
            SetUp();
            var request = new UserDTO()
            {
                Id = Guid.NewGuid()
            };

            _mockRepository.CallBase = true;

            _mockRepository.Setup(x => x.GetUser(request.Id))
                                     .ReturnsAsync((User)null);

            var actual = await _target.GetUser(request.Id);

            Assert.Null(actual);
        }

        [Fact]
        public void GetUser_ThrowsException()
        {
            SetUp();
            var request = new UserDTO()
            {
                Id = Guid.NewGuid()
            };

            _mockRepository.CallBase = true;

            _mockRepository.Setup(x => x.GetUser(request.Id))
                                     .Throws<Exception>();

            Assert.ThrowsAsync<Exception>(async () => await _target.GetUser(request.Id));
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
                Dob = DateTime.Now,
            };

            var entity = _mapper.Map<User>(request);

            _list.Add(entity);

            _mockRepository.CallBase = true;

            _mockRepository.Setup(x => x.AddUser(It.IsAny<User>()))
                .ReturnsAsync(entity);

            var actual = await _target.AddUser(request);

            Assert.IsType<UserDTO>(actual);
        }

        [Fact]
        public void AddUser_ThrowsException()
        {
            SetUp();
            var request = new AddUserRequest()
            {
                Name = "New User",
                Email = "user@email.com",
                Address = "user address",
                Dob = DateTime.Now
            };

            var entity = _mapper.Map<User>(request);

            _mockRepository.CallBase = true;

            _mockRepository.Setup(x => x.AddUser(entity))
                                     .Throws<Exception>();

            Assert.ThrowsAsync<Exception>(async () => await _target.AddUser(request));
        }
        #endregion

        #region UpdateUser
        [Fact]
        public async void UpdateUser_ReturnsOkResult()
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

            var entity = _mapper.Map<User>(user);

            _mockRepository.CallBase = true;

            _mockRepository.Setup(x => x.GetUser(user.Id))
                .ReturnsAsync(entity);
            _mockRepository.Setup(x => x.UpdateUser(It.IsAny<User>()))
                .ReturnsAsync(entity);

            var actual = await _target.UpdateUser(user);

            Assert.IsType<UserDTO>(actual);
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

            var entity = _mapper.Map<User>(user);

            _mockRepository.CallBase = true;

            _mockRepository.Setup(x => x.GetUser(It.IsAny<Guid>()))
                .ReturnsAsync((User)null);
            _mockRepository.Setup(x => x.UpdateUser(entity))
                .ReturnsAsync((User)null);

            var actual = await _target.UpdateUser(user);

            Assert.Null(actual);
        }

        [Fact]
        public void UpdateUser_ThrowsException()
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

            var entity = _mapper.Map<User>(user);

            _mockRepository.CallBase = true;

            _mockRepository.Setup(x => x.GetUser(It.IsAny<Guid>()))
                .ReturnsAsync(new User());
            _mockRepository.Setup(x => x.UpdateUser(entity))
                                     .Throws<Exception>();

            Assert.ThrowsAsync<Exception>(async () => await _target.UpdateUser(user));
        }
        #endregion

        #region DeleteUser
        [Fact]
        public async void DeleteUser_ReturnsOkResult()
        {
            SetUp();
            var index = 0;

            _mockRepository.CallBase = true;

            _mockRepository.Setup(x => x.GetUser(It.IsAny<Guid>()))
                .ReturnsAsync(new User());
            _mockRepository.Setup(x => x.DeleteUser(It.IsAny<User>()))
                                     .ReturnsAsync(_list[index]);

            var actual = await _target.DeleteUser(_list[index].Id);

            Assert.IsType<UserDTO>(actual);
        }

        [Fact]
        public async void DeleteUser_ReturnsUserNotFound()
        {
            SetUp();
            var request = new User()
            {
                Id = Guid.NewGuid()
            };

            _mockRepository.CallBase = true;

            _mockRepository.Setup(x => x.GetUser(It.IsAny<Guid>()))
                .ReturnsAsync(new User());
            _mockRepository.Setup(x => x.DeleteUser(It.IsAny<User>()))
                .ReturnsAsync((User)null);

            var actual = await _target.DeleteUser(request.Id);

            Assert.Null(actual);
        }

        [Fact]
        public void DeleteUser_ThrowsException()
        {
            SetUp();
            var request = new User()
            {
                Id = Guid.NewGuid()
            };

            _mockRepository.CallBase = true;

            _mockRepository.Setup(x => x.GetUser(It.IsAny<Guid>()))
                .ReturnsAsync(new User());
            _mockRepository.Setup(x => x.DeleteUser(It.IsAny<User>()))
                                     .Throws<Exception>();

            Assert.ThrowsAsync<Exception>(async () => await _target.DeleteUser(request.Id));
        }
        #endregion
    }
}
