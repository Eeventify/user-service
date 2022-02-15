using Microsoft.VisualStudio.TestTools.UnitTesting;

using DTO_Layer;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System;

namespace User_Service.Controllers.Tests
{
    [TestClass()]
    public class LoginControllerTests
    {
        private LoginController _loginController;

        [TestInitialize()]
        public void Initialize()
        {
            _loginController = new LoginController(null, new Mocks.MockUserDAL());
        }

        [TestMethod()]
        public void GetValidUserTest()
        {
            ObjectResult? result = _loginController.GetUser(6) as OkObjectResult;
            Assert.IsTrue(result != null && (result.Value as User).Name == "Test");
        }

        [TestMethod()]
        public void GetInvalidUserTest()
        {
            ObjectResult? result = _loginController.GetUser(-1) as BadRequestObjectResult;
            Assert.IsTrue(result != null);
        }

        [TestMethod()]
        public void AttemptValidLoginTest()
        {
            ObjectResult? result = _loginController.AttemptLogin("Test", "6") as OkObjectResult;
            Assert.IsTrue(result != null && (result.Value as User).Name == "Test");
        }

        [TestMethod()]
        public void AttemptLoginWithUnknownUsernameTest()
        {
            ObjectResult? result = _loginController.AttemptLogin("Unknown", "6") as UnauthorizedObjectResult;
            Assert.IsTrue(result != null);
        }

        [TestMethod()]
        public void AttemptLoginWithWrongPasswordTest()
        {
            ObjectResult? result = _loginController.AttemptLogin("Test", "Wrong") as UnauthorizedObjectResult;
            Assert.IsTrue(result != null);
        }

        [TestMethod()]
        public void AttemptLoginWithNullUsernameTest()
        {
            ObjectResult? result = _loginController.AttemptLogin(null, "6") as BadRequestObjectResult;
            Assert.IsTrue(result != null);
        }

        [TestMethod()]
        public void AttemptLoginWithNullPasswordTest()
        {
            ObjectResult? result = _loginController.AttemptLogin("Test", null) as BadRequestObjectResult;
            Assert.IsTrue(result != null);
        }
    }
}