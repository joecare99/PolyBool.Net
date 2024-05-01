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
    public class NodeTests
    {
        [TestMethod()]
        public void EqualsTest()
        {
            var n1 = new Node<object>(5);
            var n2 = new Node<object>(7);
            Assert.AreEqual(false,n1.Equals(n2));
            Assert.AreEqual(false,n1.Equals(null));
            Assert.AreEqual(true, n1.Equals(n1));
            Assert.AreEqual(true, n2.Equals(n2));
        }

        [TestMethod()]
        public void EqualsTest1()
        {
            object n1 = new Node<object>(5);
            object n2 = new Node<object>(7);
            Assert.AreEqual(false, n1.Equals(n2));
            Assert.AreEqual(false, n1.Equals(null));
            Assert.AreEqual(true, n1.Equals(n1));
            Assert.AreEqual(true, n2.Equals(n2));
        }
    }
}