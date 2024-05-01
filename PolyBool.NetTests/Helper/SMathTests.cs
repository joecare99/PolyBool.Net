using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolyBool.Net.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyBool.Net.Helper.Tests
{
    [TestClass()]
    public class SMathTests
    {
        [DataTestMethod()]
        [DataRow(1, 2, 1)]
        [DataRow(2, 1, 1)]
        [DataRow(1d, 2d, 1d)]
        [DataRow(2d, 1d, 1d)]
        [DataRow(10f, 12f, 10f)]
        [DataRow(22f, 20f, 20f)]
        public void MinTest(IComparable a, IComparable b, object c)
        {
            Assert.AreEqual(c, SMath.Min(a, b));
        }

        [DataTestMethod()]
        [DataRow(1, 2, 2)]
        [DataRow(2, 1, 2)]
        [DataRow(1d, 2d, 2d)]
        [DataRow(2d, 1d, 2d)]
        [DataRow(10f, 12f, 12f)]
        [DataRow(22f, 20f, 22f)]
        public void MaxTest(IComparable a, IComparable b, object c)
        {
            Assert.AreEqual(c, SMath.Max(a, b));
        }

        [DataTestMethod()]
        [DataRow(1, 1)]
        [DataRow(-2, 2)]
        [DataRow(1d, 1d)]
        [DataRow(-2d, 2d)]
        [DataRow(10f, 10f)]
        [DataRow(-22f, 22f)]
        [TestMethod()]
        public void AbsTest(IComparable a, IComparable c)
        {
            if (a is int ia)
                Assert.AreEqual(c, SMath.Abs(ia));
            else if (a is double da)
                Assert.AreEqual(c, SMath.Abs(da));
            else if (a is float fa)
                Assert.AreEqual(c, SMath.Abs(fa));
        }

        [DataTestMethod()]
        [DataRow(TypeCode.Int32, 1, 2, 3)]
        [DataRow(TypeCode.Int64, 1L, 2L, 3L)]
        [DataRow(TypeCode.Double, 1d, 2d, 3d)]
        [DataRow(TypeCode.Single, 1f, 2f, 3f)]
        [DataRow(TypeCode.Decimal, 1d, 2d, 3d)]
        [DataRow(TypeCode.Byte, 1u, 2u, 0u)]
        public void AddTest(TypeCode tc,IComparable a,IComparable b,object c)
        {
            if (tc == TypeCode.Int32)
                Assert.AreEqual(c, SMath.Add((int)a, (int)b));
            else if (tc == TypeCode.Int64)
                Assert.AreEqual(c, SMath.Add((long)a, (long)b));
            else if (tc == TypeCode.Double)
                Assert.AreEqual(c, SMath.Add((double)a, (double)b));
            else if (tc == TypeCode.Single)
                Assert.AreEqual(c, SMath.Add((float)a, (float)b));
            else if (tc == TypeCode.Decimal)
                Assert.AreEqual((decimal)(double)c, SMath.Add((decimal)(double)a, (decimal)(double)b));
            else if (tc == TypeCode.Byte)
                Assert.AreEqual((byte)(uint)c, SMath.Add((byte)(uint)a, (byte)(uint)b));
        }

        [DataTestMethod()]
        [DataRow(TypeCode.Int32, 1, 2, -1)]
        [DataRow(TypeCode.Int64, 1L, 2L, -1L)]
        [DataRow(TypeCode.Double, 1d, 2d, -1d)]
        [DataRow(TypeCode.Single, 1f, 2f, -1f)]
        [DataRow(TypeCode.Decimal, 1d, 2d, -1d)]
        [DataRow(TypeCode.Byte, 1u, 2u, 0u)]
        public void SubTest(TypeCode tc, IComparable a, IComparable b, object c)
        {
            if (tc == TypeCode.Int32)
                Assert.AreEqual(c, SMath.Sub((int)a, (int)b));
            else if (tc == TypeCode.Int64)
                Assert.AreEqual(c, SMath.Sub((long)a, (long)b));
            else if (tc == TypeCode.Double)
                Assert.AreEqual(c, SMath.Sub((double)a, (double)b));
            else if (tc == TypeCode.Single)
                Assert.AreEqual(c, SMath.Sub((float)a, (float)b));
            else if (tc == TypeCode.Decimal)
                Assert.AreEqual((decimal)(double)c, SMath.Sub((decimal)(double)a, (decimal)(double)b));
            else if (tc == TypeCode.Byte)
                Assert.AreEqual((byte)(uint)c, SMath.Sub((byte)(uint)a, (byte)(uint)b));
        }

        [DataTestMethod()]
        [DataRow(TypeCode.Int32, 1, 2, 2)]
        [DataRow(TypeCode.Int64, 1L, 2L, 2L)]
        [DataRow(TypeCode.Double, 1d, 2d, 2d)]
        [DataRow(TypeCode.Single, 1f, 2f, 2f)]
        [DataRow(TypeCode.Decimal, 1d, 2d, 2d)]
        [DataRow(TypeCode.Byte, 1u, 2u, 0u)]
        public void MulTest(TypeCode tc, IComparable a, IComparable b, object c)
        {
            if (tc == TypeCode.Int32)
                Assert.AreEqual(c, SMath.Mul((int)a, (int)b));
            else if (tc == TypeCode.Int64)
                Assert.AreEqual(c, SMath.Mul((long)a, (long)b));
            else if (tc == TypeCode.Double)
                Assert.AreEqual(c, SMath.Mul((double)a, (double)b));
            else if (tc == TypeCode.Single)
                Assert.AreEqual(c, SMath.Mul((float)a, (float)b));
            else if (tc == TypeCode.Decimal)
                Assert.AreEqual((decimal)(double)c, SMath.Mul((decimal)(double)a, (decimal)(double)b));
            else if (tc == TypeCode.Byte)
                Assert.AreEqual((byte)(uint)c, SMath.Mul((byte)(uint)a, (byte)(uint)b));
        }

        [DataTestMethod()]
        [DataRow(TypeCode.Int32, 6, 2, 3)]
        [DataRow(TypeCode.Int64, 6L, 2L, 3L)]
        [DataRow(TypeCode.Double, 6d, 2d, 3d)]
        [DataRow(TypeCode.Single, 6f, 2f, 3f)]
        [DataRow(TypeCode.Decimal, 6d, 2d, 3d)]
        [DataRow(TypeCode.Byte, 6u, 2u, 0u)]
        public void DivTest(TypeCode tc, IComparable a, IComparable b, object c)
        {
            if (tc == TypeCode.Int32)
                Assert.AreEqual(c, SMath.Div((int)a, (int)b));
            else if (tc == TypeCode.Int64)
                Assert.AreEqual(c, SMath.Div((long)a, (long)b));
            else if (tc == TypeCode.Double)
                Assert.AreEqual(c, SMath.Div((double)a, (double)b));
            else if (tc == TypeCode.Single)
                Assert.AreEqual(c, SMath.Div((float)a, (float)b));
            else if (tc == TypeCode.Decimal)
                Assert.AreEqual((decimal)(double)c, SMath.Div((decimal)(double)a, (decimal)(double)b));
            else if (tc == TypeCode.Byte)
                Assert.AreEqual((byte)(uint)c, SMath.Div((byte)(uint)a, (byte)(uint)b));
        }

        [DataTestMethod()]
        [DataRow(TypeCode.Int32, 6, -6)]
        [DataRow(TypeCode.Int32, -1, 1)]
        [DataRow(TypeCode.Int64, 6L,  -6L)]
        [DataRow(TypeCode.Int64, -1L,  1L)]
        [DataRow(TypeCode.Double, 6d, -6d)]
        [DataRow(TypeCode.Double, -1d, 1d)]
        [DataRow(TypeCode.Single, 6f, -6f)]
        [DataRow(TypeCode.Single, -1f, 1f)]
        [DataRow(TypeCode.Decimal, 6d, -6d)]
        [DataRow(TypeCode.Decimal, -1d, 1d)]
        [DataRow(TypeCode.Byte, 6u, 0u)]
        public void NegTest(TypeCode tc, IComparable a, object c)
        {
            if (tc == TypeCode.Int32)
                Assert.AreEqual(c, SMath.Neg((int)a));
            else if (tc == TypeCode.Int64)
                Assert.AreEqual(c, SMath.Neg((long)a));
            else if (tc == TypeCode.Double)
                Assert.AreEqual(c, SMath.Neg((double)a));
            else if (tc == TypeCode.Single)
                Assert.AreEqual(c, SMath.Neg((float)a));
            else if (tc == TypeCode.Decimal)
                Assert.AreEqual((decimal)(double)c, SMath.Neg((decimal)(double)a));
            else if (tc == TypeCode.Byte)
                Assert.AreEqual((byte)(uint)c, SMath.Neg((byte)(uint)a));
        }
    }
}