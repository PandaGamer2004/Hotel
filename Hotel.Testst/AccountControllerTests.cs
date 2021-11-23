using Hotel.BLogicLayer;
using Hotel.BLogicLayer.DTO;
using Hotel.BLogicLayer.Exceptions;
using Hotel.BLogicLayer.Interfaces;
using Hotel.PRLAYER.Controler;
using Hotel.PRLAYER.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using Xunit;

namespace Hotel.Testst
{
    public class AccountControllerTests
    {

        [Fact]
        public void PostRegisterUser_WithInvalidStateReturnsBadRequest()
        {
            //Arrange
            var mapper = new ContainedMapper();
            var roleMoq = new Mock<IRoleService>();
            var guestMoq = new Mock<IGuestService>();


            var controllerToTest = new AccountController(mapper, roleMoq.Object, guestMoq.Object);

            controllerToTest.ModelState.AddModelError("GuestModel", "Required");

            var guestModel = new GuestModel();

            //Act
            var res = controllerToTest.RegisterUser(guestModel);


            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(res);
            Assert.IsType<SerializableError>(badRequestResult.Value);

        }


        public RoleDto GetUserRole() => new RoleDto
        {
            RoleName = "User",
            Id = Guid.NewGuid()
        };

        public GuestModel GetGuestModel() => new GuestModel
        {
            FirstName = "Danilko",
            LastName = "Tagilko",
            Patronimic = "OtecDanko",
            BirthDate = DateTime.Now,
            PassportNumber = "123456789",

            GuestRegisterInfo = new GuestRegisterInfoModel
            {
                Login = "panda.gamer2004@gmail.com",
                Password = "12345678",
                UserName = "DanilkoTagilko",
                RoleId =Guid.Empty
            }
        };

        [Fact]
        public void PostRegisterUser_ReturnsOkResult()
        {
            //Arrange
            var mapper = new ContainedMapper();
            var roleMoq = new Mock<IRoleService>();
            var guestMoq = new Mock<IGuestService>();

            roleMoq.Setup(role => role.GetRoleByName(It.IsAny<String>()))
                .Returns(GetUserRole).Verifiable();

            guestMoq.Setup(guest => guest.CreateGuest(
                It.Is<GuestDto>(item => item.Id != Guid.Empty
               && item.GuestRegisterInfo.RoleId != GetUserRole().Id)))
                .Verifiable();

            var controllerToTest = new AccountController(mapper, roleMoq.Object, guestMoq.Object);

            var result = controllerToTest.RegisterUser(GetGuestModel());
            roleMoq.Verify();
            guestMoq.Verify();
            Assert.IsType<OkResult>(result);
        }


        [Fact]
        public void PostRegisterUser_ReturnsHttpStatusCode500()
        {
            //Arrange
            var mapper = new ContainedMapper();
            var roleMoq = new Mock<IRoleService>();
            var guestMoq = new Mock<IGuestService>();

            roleMoq.Setup(role => role.GetRoleByName(It.IsAny<String>()))
                .Throws(new KeyNotFoundException()).Verifiable();

            var controllerToTest = new AccountController(mapper,
                roleMoq.Object, guestMoq.Object);

            //Act
            var result = controllerToTest.RegisterUser(GetGuestModel());

            //Assert


            roleMoq.Verify();
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }


        [Fact]
        public void PostRegisterUser_ReturnsBadRequestWithUserEmailAlreadyContainedModelError()
        {
            //Arrange
            var mapper = new ContainedMapper();
            var roleMoq = new Mock<IRoleService>();
            var guestMoq = new Mock<IGuestService>();

            roleMoq.Setup(role => role.GetRoleByName(It.IsAny<String>()))
                .Returns(GetUserRole).Verifiable();


            guestMoq.Setup(guest => guest.CreateGuest(It.IsAny<GuestDto>()))
                .Throws(new GuestEmailAlreadyExistException("Guest already exsist"))
                .Verifiable();
            var controllerToTest = new AccountController(mapper,
               roleMoq.Object, guestMoq.Object);


            //Act
            var result = controllerToTest.RegisterUser(GetGuestModel());

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(controllerToTest.ModelState.ContainsKey("Login"));
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }


        [Fact]
        public void PostRegisterUser_ReturnsBadRequestWithUserNameAlreadyContainedModelError()
        {
            //Arrange
            var mapper = new ContainedMapper();
            var roleMoq = new Mock<IRoleService>();
            var guestMoq = new Mock<IGuestService>();

            roleMoq.Setup(role => role.GetRoleByName(It.IsAny<String>()))
                .Returns(GetUserRole).Verifiable();


            guestMoq.Setup(guest => guest.CreateGuest(It.IsAny<GuestDto>()))
                .Throws(new GuestUserNameAlreadyExistException("Guest already exsist"))
                .Verifiable();
            var controllerToTest = new AccountController(mapper,
               roleMoq.Object, guestMoq.Object);


            //Act
            var result = controllerToTest.RegisterUser(GetGuestModel());

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(controllerToTest.ModelState.ContainsKey("UserName"));
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void GetIdentity_ThrowArgumentExceptionRoleNotFound()
        {
            //Arrange
            var mapper = new ContainedMapper();
            var roleMoq = new Mock<IRoleService>();
            var guestMoq = new Mock<IGuestService>();

            roleMoq.Setup(role => role.GetRole(It.IsAny<Guid>()))
                .Throws(new ArgumentException()).Verifiable();


            guestMoq.Setup(guest => guest.GetGuestByEmailAndPassword(
                It.Is<String>(item => !String.IsNullOrEmpty(item) && !String.IsNullOrWhiteSpace(item)),
                It.Is<String>(item => !String.IsNullOrEmpty(item) && !String.IsNullOrWhiteSpace(item))))
                .Returns(mapper.Mapper.Map<GuestDto>(GetGuestModel()))
                .Verifiable();

            var controllerToTest = new AccountController(mapper,
               roleMoq.Object, guestMoq.Object);

            Assert.Throws<ArgumentException>(() => controllerToTest.GetIdentity("log", "passw"));
        }
        

        [Fact]
        public void GetIdentity_NotFoundUserAddModelErrorAndReturnsNull()
        {
            var mapper = new ContainedMapper();
            var roleMoq = new Mock<IRoleService>();
            var guestMoq = new Mock<IGuestService>();

            guestMoq.Setup(guest => guest.GetGuestByEmailAndPassword(It.IsAny<String>(), It.IsAny<String>()))
                .Throws(new KeyNotFoundException());
               

            var controllerToTest = new AccountController(
                mapper, roleMoq.Object, guestMoq.Object);

            var res = controllerToTest.GetIdentity("", "");
            Assert.Null(res);
            Assert.True(controllerToTest.ModelState.ContainsKey(""));
        }
        public LogInModel GetLogInModel() => new LogInModel { Email = "dan@gmail.com", Password = "12345" };

        [Fact]
        public void HttpPostRouteToken_GetTokenReturnsStatusCode500()
        {
            var mapper = new ContainedMapper();
            var roleMoq = new Mock<IRoleService>();
            var guestMoq = new Mock<IGuestService>();

            roleMoq.Setup(role => role.GetRole(It.IsAny<Guid>()))
                .Throws(new ArgumentException()).Verifiable();


            guestMoq.Setup(guest => guest.GetGuestByEmailAndPassword(
                It.Is<String>(item => !String.IsNullOrEmpty(item) && !String.IsNullOrWhiteSpace(item)),
                It.Is<String>(item => !String.IsNullOrEmpty(item) && !String.IsNullOrWhiteSpace(item))))
                .Returns(mapper.Mapper.Map<GuestDto>(GetGuestModel()))
                .Verifiable();

            var controllerToTest = new AccountController(mapper,
               roleMoq.Object, guestMoq.Object);

            var res = controllerToTest.GetToken(GetLogInModel());

            roleMoq.Verify();
            guestMoq.Verify();
            var statusCodeResult = Assert.IsType<StatusCodeResult>(res);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public void HttpPostRouteToken_GetTokenWithInvalidStateReturnBadRequestObjectResult()
        {
            var mapper = new ContainedMapper();
            var roleMoq = new Mock<IRoleService>();
            var guestMoq = new Mock<IGuestService>();


            var controllerToTest = new AccountController(mapper, roleMoq.Object, guestMoq.Object);

            controllerToTest.ModelState.AddModelError("LogInModel", "Required");

            var res = controllerToTest.GetToken(GetLogInModel());

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(res);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }


        [Fact]
        public void GetIdentity_ReturnsClaimsIdentityNotNull()
        {
            var mapper = new ContainedMapper();
            var roleMoq = new Mock<IRoleService>();
            var guestMoq = new Mock<IGuestService>();

            roleMoq.Setup(role => role.GetRole(It.IsAny<Guid>()))
                .Returns(GetUserRole());

            guestMoq.Setup(guest => guest.GetGuestByEmailAndPassword(It.IsAny<String>(), It.IsAny<String>()))
                .Returns(mapper.Mapper.Map<GuestDto>(GetGuestModel()));
               

            var controllerToTest = new AccountController(mapper, roleMoq.Object, guestMoq.Object);
            var res = controllerToTest.GetIdentity(GetLogInModel().Email, GetLogInModel().Password);


            Assert.IsType<ClaimsIdentity>(res);
            Assert.NotNull(res);
        }

        [Fact]
        public void HttpPostRouteToken_ReturnsJsonResultWithNotNullToken()
        {
            var mapper = new ContainedMapper();
            var roleMoq = new Mock<IRoleService>();
            var guestMoq = new Mock<IGuestService>();

            roleMoq.Setup(role => role.GetRole(It.IsAny<Guid>()))
                .Returns(GetUserRole());

            guestMoq.Setup(guest => guest.GetGuestByEmailAndPassword(It.IsAny<String>(), It.IsAny<String>()))
                .Returns(mapper.Mapper.Map<GuestDto>(GetGuestModel()));


            var controllerToTest = new AccountController(mapper, roleMoq.Object, guestMoq.Object);


            var res = controllerToTest.GetToken(GetLogInModel());

            var jsonResultFromController = Assert.IsType<JsonResult>(res);
        }

    }

   


}
