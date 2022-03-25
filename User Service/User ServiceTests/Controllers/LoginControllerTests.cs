#pragma warning disable
using Microsoft.VisualStudio.TestTools.UnitTesting;

using DTO_Layer;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System;

using Abstraction_Layer;

namespace User_Service.Controllers.Tests
{
    [TestClass()]
    public class LoginControllerTests
    {
        private LoginController _loginController;
        private Mocks.MockUserDAL _mockUserDAL;

        [TestInitialize()]
        public void Initialize()
        {
            _mockUserDAL = new Mocks.MockUserDAL();
            _loginController = new LoginController(_mockUserDAL);
        }


        [TestMethod()]
        public void AttemptValidLoginTest()
        {
            ObjectResult? result = _loginController.AttemptLogin(_mockUserDAL.users[5].Email, "6") as OkObjectResult;
            Assert.IsTrue(result != null);
        }

        [TestMethod()]
        public void AttemptLoginWithUnknownEmailTest()
        {
            ObjectResult? result = _loginController.AttemptLogin("Unknown", "6") as UnauthorizedObjectResult;
            Assert.IsTrue(result != null);
        }

        [TestMethod()]
        public void AttemptLoginWithWrongPasswordTest()
        {
            ObjectResult? result = _loginController.AttemptLogin(_mockUserDAL.users[5].Email, "Wrong") as UnauthorizedObjectResult;
            Assert.IsTrue(result != null);
        }

        [TestMethod()]
        public void AttemptLoginWithNullEmailTest()
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