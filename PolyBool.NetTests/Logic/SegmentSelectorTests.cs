using System.Collections.Generic;
using PolyBool.Net.Interfaces;

namespace PolyBool.Net.Logic.Tests
{
    [TestClass()]
    public class SegmentSelectorTests
    {
        [DataTestMethod()]
        [DataRow(new double[] { }, 0, "")]
        [DataRow(new double[] { 0, 0 }, 1, "(0, 0)")]
        [DataRow(new double[] { 0, 0, 8, 8 }, 2, "(0, 0), (8, 8)")]
        [DataRow(new double[] { 0, 0, 16, 0, 8, 8 }, 3, "(0, 0), (16, 0), (8, 8)")]
        public void Val2PntsTest(double[] pvals, int ex, string sExp)
        {

            var pnts = PolyBoolTestsHelpers.Val2Pnts(pvals);
            Assert.IsInstanceOfType(pnts, typeof(IList<IPoint>));
            Assert.AreEqual(ex, pnts.Count);
            Assert.AreEqual(sExp, string.Join(", ", pnts));
        }

        [DataTestMethod()]
        [DataRow(new object[] { new double[] { 0, 0, 16, 0, 8, 8 } },
                 new object[] { new double[] { 16, 6, 8, 14, 0, 6 } },
                new object[] { new double[] { 16, 6, 10, 6, 16, 0, 0, 0, 6, 6, 0, 6, 8, 14 } })]
        [DataRow(new object[] { new double[] { 0, 0, 16, 0, 8, 8 } },
                 new object[] { new double[] { 2, 1, 14, 1, 8, 7 } },
                new object[] { new double[] { 16, 0, 0, 0, 8, 8 } })]
        public void UnionTest(object[] p1, object[] p2, object[] ex)
        {
            var pg1 = p1.Val2Poly();
            var pg2 = p2.Val2Poly();
            var unified = SegmentSelector.Union(pg1, pg2);
            Assert.IsInstanceOfType(unified, typeof(IPolygon));
            Assert.AreEqual(ex.Length, unified.Regions.Count);
            for (int i = 0; i < ex.Length; i++)
            {
                var region = unified.Regions[i];
                Assert.IsInstanceOfType(region, typeof(IRegion));
                Assert.AreEqual((ex[i] as IList<double>).Count, region.Points.Count * 2);
                for (int j = 0; j < (ex[i] as IList<double>).Count - 1; j += 2)
                {
                    Assert.AreEqual((ex[i] as IList<double>)[j], region.Points[j / 2].X, $"R[{i}].P[{j / 2}].X");
                    Assert.AreEqual((ex[i] as IList<double>)[j + 1], region.Points[j / 2].Y, $"R[{i}].P[{j / 2}].X");
                }
            }
        }

        [DataTestMethod()]
        [DataRow(new object[] { new double[] { 0, 0, 16, 0, 8, 8 } },
             new object[] { new double[] { 16, 6, 8, 14, 0, 6 } },
            new object[] { new double[] { 16, 0, 0, 0, 6, 6, 10, 6 } })]
        [DataRow(new object[] { new double[] { 0, 0, 16, 0, 8, 8 } },
             new object[] { new double[] { 2, 1, 14, 1, 8, 7 } },
            new object[] { new double[] { 14, 1, 2, 1, 8, 7 }, new double[] { 16, 0, 0, 0, 8, 8 } })]
        public void DifferenceTest(object[] p1, object[] p2, object[] ex)
        {
            var pg1 = p1.Val2Poly();
            var pg2 = p2.Val2Poly();
            var unified = SegmentSelector.Difference(pg1, pg2);
            Assert.IsInstanceOfType(unified, typeof(IPolygon));
            Assert.AreEqual(ex.Length, unified.Regions.Count);
            for (int i = 0; i < ex.Length; i++)
            {
                var region = unified.Regions[i];
                Assert.IsInstanceOfType(region, typeof(IRegion));
                Assert.AreEqual((ex[i] as IList<double>).Count, region.Points.Count * 2);
                for (int j = 0; j < (ex[i] as IList<double>).Count - 1; j += 2)
                {
                    Assert.AreEqual((ex[i] as IList<double>)[j], region.Points[j / 2].X, $"R[{i}].P[{j / 2}].X");
                    Assert.AreEqual((ex[i] as IList<double>)[j + 1], region.Points[j / 2].Y, $"R[{i}].P[{j / 2}].Y");
                }
            }
        }
    }
}