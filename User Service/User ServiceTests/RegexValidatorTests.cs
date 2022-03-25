using Microsoft.VisualStudio.TestTools.UnitTesting;
using User_Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS8618

namespace User_Service.Tests
{
    [TestClass()]
    public class RegexValidatorTests
    {
        RegexValidator validator;


        [TestInitialize()]
        public void Initialize()
        {
            validator = new RegexValidator();
        }

        [TestMethod()]
        public void ValidUsernameValidityTest()
        {
            bool validity = validator.Username("Test");
            Assert.IsTrue(validity);
        }

        [TestMethod()]
        public void ValidEmailValidityTest()
        {
            bool validity = validator.Email("Test@domain.com");
            Assert.IsTrue(validity);
        }

        [TestMethod()]
        public void ValidPasswordValidityTest()
        {
            bool validity = validator.Password("Letmein");
            Assert.IsTrue(validity);
        }

        [TestMethod()]
        public void IllegalCharacterInUsernameValidityTest()
        {
            bool validity = validator.Username("*_Invalid_*");
            Assert.IsFalse(validity);
        }

        [TestMethod()]
        public void BlankUsernameValidityTest()
        {
            bool validity = validator.Username("");
            Assert.IsFalse(validity);
        }

        [TestMethod()]
        public void TooShortUsernameValidityTest()
        {
            bool validity = validator.Username("0");
            Assert.IsFalse(validity);
        }

        [TestMethod()]
        public void TooLongUsernameValidityTest()
        {
            bool validity = validator.Username("ThisIsAVeryLongUsernameThatShouldNeverBeAllowedToBeUsedOnAWebsiteBecouseItIsSimplyTooLongToDisplayInAnyPropperWay");
            Assert.IsFalse(validity);
        }


        [TestMethod()]
        public void NoDomainEmailValidityTest()
        {
            bool validity = validator.Email("NoDomain.com");
            Assert.IsFalse(validity);
        }

        [TestMethod()]
        public void NoCountryCodeEmailValidityTest()
        {
            bool validity = validator.Email("No@Countrycode");
            Assert.IsFalse(validity);
        }

        [TestMethod()]
        public void BlankEmailValidityTest()
        {
            bool validity = validator.Email("");
            Assert.IsFalse(validity);
        }

        [TestMethod()]
        public void BlankPasswordValidityTest()
        {
            bool validity = validator.Password("");
            Assert.IsFalse(validity);
        }

        [TestMethod()]
        public void IllegalCharacterInPasswordValidityTest()
        {
            bool validity = validator.Password("*_=Illegal=_*");
            Assert.IsFalse(validity);
        }

        [TestMethod()]
        public void TooShortPasswordValidityTest()
        {
            bool validity = validator.Password("0");
            Assert.IsFalse(validity);
        }

        [TestMethod()]
        public void TooLongPasswordValidityTest()
        {
            bool validity = validator.Password("ThisIsAVeryLongPassThatShouldNeverBeAllowedToBeUsedOnAWebsiteBecouseItIsSimplyTooLongForPeopleToRemember");
            Assert.IsFalse(validity);
        }
    }
}