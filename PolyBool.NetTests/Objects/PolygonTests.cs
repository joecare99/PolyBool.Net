using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolyBool.Net.Interfaces;
using PolyBool.Net.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyBool.Net.Objects.Tests
{
    [TestClass()]
    public class PolygonTests
    {
        [TestMethod()]
        public void PolygonTest()
        {
            IPolygon? polygon;
            polygon = new Polygon();
            Assert.IsNotNull(polygon);
            Assert.IsInstanceOfType(polygon, typeof(IPolygon));
            Assert.IsInstanceOfType(polygon, typeof(Polygon));
            Assert.AreEqual(0, polygon.Regions.Count);
            Assert.IsFalse(polygon.Inverted);
        }
    }
}