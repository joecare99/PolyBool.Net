using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class PolyBoolTests
    {
        [DataTestMethod()]
        [DataRow(new object[] { new double[] { 0, 0, 16, 0, 8, 8 } },
                           new double[] { 0, 0, 8, 8, 4, 0, 0, 16, 0,8, 8, 8, 16, 0,  4 })]
        [DataRow(new object[] { new double[] { 0, 0, 8, 8, 16, 0 } },
                           new double[] { 0, 0, 8, 8, 4, 0, 0, 16, 0, 8, 8, 8, 16, 0, 4 })]
        public void SegmentsTest(object[] p1, double[] aEx)
        {
            var pl1 = p1.Val2Poly();
            var segments = PolyBool.Segments(pl1);
            Assert.IsInstanceOfType(segments, typeof(PolySegments));
            Assert.AreEqual(aEx.Length, segments.Segments.Count*5);
            for (int i = 0; i < aEx.Length - 4; i += 5 )
            {
                var s = segments.Segments[i/5];
                Assert.IsInstanceOfType(s, typeof(ISegment));
                Assert.AreEqual((decimal)aEx[i], s.Start.X,$"S[{i/5}].S.X");
                Assert.AreEqual((decimal)aEx[i + 1], s.Start.Y, $"S[{i / 5}].S.Y");
                Assert.AreEqual((decimal)aEx[i + 2], s.End.X, $"S[{i / 5}].E.X");
                Assert.AreEqual((decimal)aEx[i + 3], s.End.Y, $"S[{i / 5}].EY");
                Assert.AreEqual((int)aEx[i + 4], s.FIndex(), $"S[{i / 5}].IX");
            }
        }
    }
}