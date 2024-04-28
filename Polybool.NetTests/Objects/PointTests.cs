using PolyBool.Net.Interfaces;

namespace PolyBool.Net.Objects.Tests
{
    [TestClass()]
    public class PointTests
    {
        private readonly Point Zero = new Point(0, 0);
        private readonly Point OneOne = new Point(1, 1);
        private int ZeroHash => Zero.GetHashCode();

        [DataTestMethod()]
        [DataRow(0,0,1,1,1,1)]
        [DataRow(1, 1, 0, 0, 1, 1)]
        [DataRow(1, 3, 2, 5, 3, 8)]
        public void AddTest(double X,double Y, double X1, double Y1, double ExpX, double ExpY)
        {
            IPoint<double> p = new Point(X, Y);
            Point p1 = new Point(X1, Y1);
            Assert.IsInstanceOfType(p.Add(p1),typeof(IPoint));
            Assert.AreEqual(ExpX, p.X);
            Assert.AreEqual(ExpY, p.Y);
        }

        [DataTestMethod()]
        [DataRow(0, 0, 1, 1, false)]
        [DataRow(1, 1, 0, 0, false)]
        [DataRow(1, 1, 1, 1, true)]
        [DataRow(1, 2, 1, 2, true)]
        [DataRow(1, 2, 2, 1, false)]
        public void EqualsTest(double X,double Y,double X1,double Y1,bool xExp)
        {
            Point p = new Point(X, Y);
            Point p1 = new Point(X1, Y1);
            Assert.AreEqual(xExp,p.Equals(p1));
        }

        [DataTestMethod()]
        [DataRow(0, 0, "1, 1", false)]
        [DataRow(0, 0, null, false)]
        public void EqualsTest2(double X, double Y, object? o, bool xExp)
        {
            Point p = new Point(X, Y);
            Assert.AreEqual(xExp, p.Equals(o));
        }
        [DataTestMethod()]
        [DataRow(0, 0, 1, 0, 0)]
        [DataRow(1, 1, 0, 0, 0)]
        [DataRow(1, 3, 2, 2, 6)]
        [DataRow(3, 2, -1, -3, -2)]
        public void MultiplyTest(double X, double Y, double f, double ExpX, double ExpY)
        {
            IPoint<double> p = new Point(X, Y);
            Assert.IsInstanceOfType(p.Multiply(f),typeof(IPoint));
            Assert.AreEqual(ExpX, p.X);
            Assert.AreEqual(ExpY, p.Y);
        }

        [DataTestMethod()]
        [DataRow(0, 0, 1, 2, 0)]
        [DataRow(1, 0, 0, 1, 0)]
        [DataRow(1, 3, 2, 2, 8)]
        [DataRow(3, 2, -1, -3, -9)]
        public void Multiply2Test(double X, double Y, double X1, double X2, double dExp)
        {
            IPoint<double> p = new Point(X, Y);
            Point p1 = new Point(X1, X2);
            Assert.AreEqual(dExp, p.Multiply(p1), 1e-5);
        }

        [DataTestMethod()]
        [DataRow(0, 0, 1, 2, 0 , 0)]
        [DataRow(1, 0, 0, 1, 0, 1)]
        [DataRow(1, 3, 2, 2, -4,8)]
        [DataRow(3, 2, -1, -3, 3,-11)]
        public void CMultiplyTest(double X, double Y, double X1, double X2, double ExpX, double ExpY)
        {
            IPoint<double> p = new Point(X, Y);
            Point p1 = new Point(X1, X2);
            var erg= p.CMultiply(p1);
            Assert.IsInstanceOfType(erg, typeof(IPoint));
            Assert.AreEqual(ExpX,erg.X , 1e-5);
            Assert.AreEqual(ExpY,erg.Y , 1e-5);
        }

        [DataTestMethod()]
        [DataRow(0, 0, 1, 1, -1, -1)]
        [DataRow(1, 1, 0, 0, 1, 1)]
        [DataRow(1, 3, 2, 5, -1, -2)]
        public void SubtractTest(double X, double Y, double X1, double Y1, double ExpX, double ExpY)
        {
            IPoint<double> p = new Point(X, Y);
            Point p1 = new Point(X1, Y1);
            Assert.IsInstanceOfType(p.Subtract(p1), typeof(IPoint));
            Assert.AreEqual(ExpX, p.X);
            Assert.AreEqual(ExpY, p.Y);
        }

        [DataTestMethod()]
        [DataRow(0,0,"(0, 0)")]
        [DataRow(1,1,"(1, 1)")]
        [DataRow(1,3,"(1, 3)")]
        [DataRow(3,2,"(3, 2)")]
        public void ToStringTest(double X,double Y,string sExp)
        {
            Point p = new Point(X, Y);
            Assert.AreEqual(sExp,p.ToString());
        }

        [DataTestMethod()]
        [DataRow(0, 0,  true)]
        [DataRow(1, 1,  false)]
        [DataRow(1, 3,  false)]
        [DataRow(3, 2,  false)]
        public void GetHashCodeTest0(double X, double Y,bool xExpZero )
        {
            Point p = new Point(X, Y);
            Assert.AreEqual(xExpZero,p.GetHashCode() == ZeroHash);
        }

        [DataTestMethod()]
        [DataRow(0, 0, false)]
        [DataRow(1, 1, true)]
        [DataRow(1, 3, false)]
        [DataRow(3, 2, false)]
        public void GetHashCodeTest1(double X, double Y, bool xExpZero)
        {
            Point p = new Point(X, Y);
            Assert.AreEqual(xExpZero, p.GetHashCode() == OneOne.GetHashCode());
        }

        [DataTestMethod()]
        [DataRow(0, 0, 1, 1, false)]
        [DataRow(1, 1, 0, 0, false)]
        [DataRow(1, 1, 1, 1, true)]
        [DataRow(1, 2, 1, 2, true)]
        [DataRow(1, 2, 2, 1, false)]
        public void GetHashCodeTestE(double X, double Y, double X1, double Y1, bool xExp)
        {
            Point p = new Point(X, Y);
            Point p1 = new Point(X1, Y1);
            Assert.AreEqual(xExp, p.GetHashCode()== p1.GetHashCode());
        }

        [DataTestMethod()]
        [DataRow(0, 0, 1, 1, false)]
        [DataRow(1, 1, 0, 0, false)]
        [DataRow(1, 1, 1 + 1e-9, 1 + 1e-9, true)]
        [DataRow(1, 2, 1 + 1e-9, 2 + 1e-9, true)]
        [DataRow(1, 2, 1 + 1e-9, 1, true)]
        [DataRow(2, 2, 1 + 1e-9, 2- 1e-9, false)]
        public void IsSameXTest(double X, double Y, double X1, double Y1, bool xExp)
        {
            Point p = new Point(X, Y);
            Point p1 = new Point(X1, Y1);
            Assert.AreEqual(xExp, p.SameX(p1,0.01));
        }

        [DataTestMethod()]
        [DataRow(0, 0, 1, 1, false)]
        [DataRow(1, 1, 0, 0, false)]
        [DataRow(1, 1, 1 + 1e-9, 1 + 1e-9, true)]
        [DataRow(1, 2, 1 + 1e-9, 2 + 1e-9, true)]
        [DataRow(1, 2, 1 + 1e-9, 1, false)]
        [DataRow(2, 2, 1 + 1e-9, 2 - 1e-9, true)]
        public void IsSameYTest(double X, double Y, double X1, double Y1, bool xExp)
        {
            Point p = new Point(X, Y);
            Point p1 = new Point(X1, Y1);
            Assert.AreEqual(xExp, p.SameY(p1, 0.01));
        }

        [DataTestMethod()]
        [DataRow(0, 0, 1, 1, false)]
        [DataRow(1, 1, 0, 0, false)]
        [DataRow(1, 1, 1 + 1e-9, 1 + 1e-9, true)]
        [DataRow(1, 2, 1 + 1e-9, 2 + 1e-9, true)]
        [DataRow(1, 2, 1 + 1e-9, 1, false)]
        [DataRow(2, 2, 1 + 1e-9, 2 - 1e-9, false)]
        public void IsSameTest(double X, double Y, double X1, double Y1, bool xExp)
        {
            Point p = new Point(X, Y);
            Point p1 = new Point(X1, Y1);
            Assert.AreEqual(xExp, p.Same(p1, 0.01));
        }

        [DataTestMethod()]
        [DataRow(0, 0 )]
        [DataRow(1, 1)]
        [DataRow(1, 1)]
        [DataRow(1, 2)]
        [DataRow(1, 2)]
        public void CloneTest(double X, double Y)
        {
            IPoint<double> p = new Point(X, Y);
            var p1 = p.Clone();
            Assert.AreEqual(false, p == p1);
            Assert.AreEqual(true, p.Equals(p1));
        }

        [DataTestMethod()]
        [DataRow(0, 0, 0, 0)]
        [DataRow(1, 1, -1, 1)]
        [DataRow(-1, 1, -1, -1)]
        [DataRow(1, 2, -2, 1)]
        [DataRow(1, -2, 2, 1)]
        public void NormalTest(double X, double Y, double ExpX, double ExpY)
        {
            IPoint<double> p = new Point(X, Y);
            Assert.IsInstanceOfType(p.Normal(), typeof(IPoint));
            Assert.AreEqual(ExpX, p.X,"X");
            Assert.AreEqual(ExpY, p.Y, "Y");
        }
    }
}