using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolyBool.Net.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyBool.Net.Objects.Tests
{
    [TestClass()]
    public class EpsilonTests
    {
        [TestMethod()]
        public void EpsTest()
        {
            Assert.AreNotEqual(0d,Epsilon<double>.Eps);
            Assert.IsInstanceOfType(Epsilon<double>.Eps,typeof(double));
        }
    }
}