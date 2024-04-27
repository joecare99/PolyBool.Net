using Polybool.Net.Interfaces;

namespace Polybool.Net.Objects.Tests
{
    [TestClass()]
    public class PointTests
    {
        [DataTestMethod()]
        [DataRow(0,0,1,1,1,1)]
        [DataRow(1, 1, 0, 0, 1, 1)]
        [DataRow(1, 3, 2, 5, 3, 8)]
        public void AddTest(double X,double Y, double X1, double Y1, double ExpX, double ExpY)
        {
            Point p = new Point(X, Y);
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
        [DataRow(0, 0, 1, 0, 0)]
        [DataRow(1, 1, 0, 0, 0)]
        [DataRow(1, 3, 2, 2, 6)]
        [DataRow(3, 2, -1, -3, -2)]
        public void MultiplyTest(double X, double Y, double f, double ExpX, double ExpY)
        {
            Point p = new Point(X, Y);
            Assert.IsInstanceOfType(p.Multiply(f),typeof(IPoint));
            Assert.AreEqual(ExpX, p.X);
            Assert.AreEqual(ExpY, p.Y);
        }

        [DataTestMethod()]
        [DataRow(0, 0, 1, 1, -1, -1)]
        [DataRow(1, 1, 0, 0, 1, 1)]
        [DataRow(1, 3, 2, 5, -1, -2)]
        public void SubtractTest(double X, double Y, double X1, double Y1, double ExpX, double ExpY)
        {
            Point p = new Point(X, Y);
            Point p1 = new Point(X1, Y1);
            Assert.IsInstanceOfType(p.Subtract(p1), typeof(IPoint));
            Assert.AreEqual(ExpX, p.X);
            Assert.AreEqual(ExpY, p.Y);
        }

        [DataTestMethod()]
        [DataRow(0,0,"(0,0)")]
        [DataRow(1,1,"(1,1)")]
        [DataRow(1,3,"(1,3)")]
        [DataRow(3,2,"(3,2)")]
        public void ToStringTest(double X,double Y,string sExp)
        {
            Point p = new Point(X, Y);
            Assert.AreEqual(sExp,p.ToString());
        }

        [TestMethod()]
        public void GetHashCodeTest()
        {
            Assert.Fail();
        }
    }
}