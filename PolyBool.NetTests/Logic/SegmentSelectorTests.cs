using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolyBool.Net.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolyBool.Net.Interfaces;
using PolyBool.Net.Objects;

namespace PolyBool.Net.Logic.Tests
{
    [TestClass()]
    public class SegmentSelectorTests
    {
        private IList<IPoint> Val2Pnts(IList<double> pvals)
        {
            var result = new List<IPoint>();
            for (int i = 0; i < pvals.Count - 1; i += 2)
            {
                result.Add(Point.New((decimal)pvals[i], (decimal)pvals[i + 1]));
            }
            return result;
        }

        [DataTestMethod()]
        [DataRow(new double[] {  } ,0,"")]
        [DataRow(new double[] { 0, 0 } ,1, "(0, 0)")]
        [DataRow(new double[] { 0, 0, 8, 8 } ,2, "(0, 0), (8, 8)")]
        [DataRow(new double[] { 0, 0, 16, 0, 8, 8 } ,3, "(0, 0), (16, 0), (8, 8)")]
        public void Val2PntsTest(double[] pvals, int ex, string sExp)
        {
            var pnts = Val2Pnts(pvals);
            Assert.IsInstanceOfType(pnts, typeof(IList<IPoint>));
            Assert.AreEqual(ex, pnts.Count);
            Assert.AreEqual(sExp, string.Join(", ",pnts));
        }

        [DataTestMethod()]
        [DataRow(new object[] { new double[] { 0, 0, 16, 0, 8, 8 } },
                 new object[] { new double[] { 16, 6, 8, 14, 0, 6 } },
                new object[] { new double[] { 16, 6, 10, 6, 16, 0, 0, 0, 6, 6, 0, 6 ,8,14} })]
        public void UnionTest(object[] p1, object[] p2, object[] ex)
        {
            var pg1 = Polygon.New([
                Region.New(Val2Pnts(p1[0] as IList<double>)
                                   )], false);
            var pg2 = Polygon.New([
                Region.New(Val2Pnts(p2[0] as IList<double>)
                                   )], false);
            var unified = SegmentSelector.Union(pg1, pg2);
            Assert.IsInstanceOfType(unified, typeof(IPolygon));
            Assert.AreEqual(ex.Length, unified.Regions.Count);
            for (int i = 0; i < ex.Length; i++)
            {
                var region = unified.Regions[i];
                Assert.IsInstanceOfType(region, typeof(IRegion));
                Assert.AreEqual((ex[i] as IList<double>).Count, region.Points.Count*2);
                for (int j = 0; j < (ex[i] as IList<double>).Count-1; j += 2)
                {
                    Assert.AreEqual((decimal)(ex[i] as IList<double>)[j], region.Points[j / 2].X,$"R[{i}].P[{j/2}].X");
                    Assert.AreEqual((decimal)(ex[i] as IList<double>)[j + 1], region.Points[j / 2].Y, $"R[{i}].P[{j/2}].X");
                }
            }
        }
    }
}