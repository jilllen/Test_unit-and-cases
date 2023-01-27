using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using cour;

namespace UnitTestCRC
{
    [TestClass]
    public class CRCTest
    {
        string text = "6B 69 6E 64";
        string polynom = "x^8+x^7+x^6+x^4+x^2+1";
        CRC CRC_Y;

        [TestInitialize]
        public void Initialize()
        {
            CRC_Y = new CRC(text, polynom);
        }

        [TestMethod]
        public void ComputeCRC_Test()
        {
            Assert.AreEqual("01001010", CRC_Y.Compute());
        }
        
        [TestMethod]
        public void CheckIntegrity_WithZeroRemainderTest()
        {
            string textWithCRC = CRC_Y.BinaryText + CRC_Y.Compute();
            var dataIntegrity = new CRC();
            string remainder = dataIntegrity.CheckMessageIntegrity(textWithCRC, CRC_Y.GeneratingPolynom);

            Assert.IsFalse(remainder.Contains("1"));
        }

        [TestMethod]
        public void CheckIntegrity_WithNonZeroRemainderTest()
        {
            string textWithCRC = "1" + CRC_Y.BinaryText + "1" + CRC_Y.Compute();
            var dataIntegrity = new CRC();
            string remainder = dataIntegrity.CheckMessageIntegrity(textWithCRC, CRC_Y.GeneratingPolynom);

            Assert.IsTrue(remainder.Contains("1"));
        }
        
    }
}
