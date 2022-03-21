#pragma warning disable
using Microsoft.VisualStudio.TestTools.UnitTesting;
using User_Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User_Service.Tests
{
    [TestClass()]
    public class HashManagerTests
    {

        [TestMethod()]
        public void GetValidHashTest()
        {
            Assert.AreEqual("532eaabd9574880dbf76b9b8cc00832c20a6ec113d682299550d7a6e0f345e25", HashManager.GetHash("Test"));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetNullHashTest()
        {
            HashManager.GetHash(null);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetEmptyHashTest()
        {
            HashManager.GetHash("");
        }

        [TestMethod()]
        public void CompareValidStringToMatchingHashTest()
        {
            Assert.IsTrue(HashManager.CompareStringToHash("Test", "532eaabd9574880dbf76b9b8cc00832c20a6ec113d682299550d7a6e0f345e25"));
        }

        [TestMethod()]
        public void CompareValidStringToNonMatchingHashTest()
        {
            Assert.IsFalse(HashManager.CompareStringToHash("Test", "Wrong"));
        }

        [TestMethod()]
        public void CompareNullStringToHashTest()
        {
            Assert.IsFalse(HashManager.CompareStringToHash(null, "Test"));
        }

        [TestMethod()]
        public void CompareValidStringToNullHashTest()
        {
            Assert.IsFalse(HashManager.CompareStringToHash("Test", null));
        }

        [TestMethod()]
        public void CompareNullStrinToNullHashTest()
        {
            Assert.IsFalse(HashManager.CompareStringToHash(null, null));
        }
    }
}