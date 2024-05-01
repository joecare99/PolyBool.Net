using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolyBool.Net.Interfaces;
using PolyBool.Net.Logic;
using PolyBool.Net.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyBool.Net.Logic.Tests
{
    [TestClass()]
    public class PointUtilsTests
    {
        [DataTestMethod()]
        [DataRow(new double[] { 0, 0, 0, 0, 0, 0 }, false)]
        [DataRow(new double[] { 0, 0, 1, 0, 0, 2 }, true)] //?!
        [DataRow(new double[] { 1, 1, 0, 0, 2, 2 }, true)]
        [DataRow(new double[] { 0, 1, -1, 0, 1, 0 }, true)] //?!
        [DataRow(new double[] { 1, 1, -1, 0, 1, 0 }, false)]
        [DataRow(new double[] { -1, 1, -1, 0, 1, 0 }, false)]
        public void PointBetweenTest(double[] adP, bool xExp)
        {
            var p = adP.Val2Pnts();
            Assert.AreEqual(xExp, PointUtils.PointBetween(p[0], p[1], p[2]));
        }

        [TestMethod()]
        [DataRow(new double[] { 0, 0, 0,0 }, true)]
        [DataRow(new double[] { 0, 0, 1,0 }, false)]
        [DataRow(new double[] { 0, 0, 0,1 }, false)]
        [DataRow(new double[] { 0, 0, 0.0001,0 }, true)]
        [DataRow(new double[] { 0, 0, 0,0.0001 }, true)]
        public void SameTTest(double[] adP, bool xExp)
        {
            var p = adP.Val2Pnts();
            IPoint<decimal> p1 = p[0];    
            Assert.AreEqual(true,p1.Same(p[0],0.01m));
            Assert.AreEqual(xExp,p1.Same(p[1],0.01m));
        }
    }
}