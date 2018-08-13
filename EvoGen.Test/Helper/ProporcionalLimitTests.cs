using EvoGen.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EvoGen.Test.Helper
{
    [TestClass]
    public class ProporcionalLimitTests
    {
        [TestMethod]
        public void GetValueByPoint_BeginRange()
        {
            var pl1 = new ProporcionalLimit(0, 49, 1, 15);
            var value1 = pl1.GetValueByPoint(0.00);
            Assert.AreEqual(1, value1);

            var pl2 = new ProporcionalLimit(0, 60, 10, 30);
            var value2 = pl2.GetValueByPoint(0.00);
            Assert.AreEqual(10, value2);

            var pl3 = new ProporcionalLimit(0, 2, 0, 5);
            var value3 = pl3.GetValueByPoint(0.00);
            Assert.AreEqual(0, value3);
        }

        [TestMethod]
        public void GetValueByPoint_EndRange()
        {
            var pl1 = new ProporcionalLimit(0, 49, 1, 15);
            var value1 = pl1.GetValueByPoint(49.00);
            Assert.AreEqual(15, value1);

            var pl2 = new ProporcionalLimit(0, 60, 10, 30);
            var value2 = pl2.GetValueByPoint(60.00);
            Assert.AreEqual(30, value2);

            var pl3 = new ProporcionalLimit(0, 2, 0, 5);
            var value3 = pl3.GetValueByPoint(2.00);
            Assert.AreEqual(5, value3);
        }

        [TestMethod]
        public void GetValueByPoint_MidRange()
        {
            var pl1 = new ProporcionalLimit(0, 49, 1, 15);
            var value1 = pl1.GetValueByPoint(25.00);
            Assert.AreEqual(8, value1);

            var pl2 = new ProporcionalLimit(0, 60, 10, 30);
            var value2 = pl2.GetValueByPoint(30.00);
            Assert.AreEqual(20, value2);

            var pl3 = new ProporcionalLimit(0, 2, 0, 5);
            var value3 = pl3.GetValueByPoint(1.00);
            Assert.AreEqual(2, value3);
        }
    }
}
