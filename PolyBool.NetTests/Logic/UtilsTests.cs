using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolyBool.Net.Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyBool.Net.Logic.Tests
{
    [TestClass()]
    public class UtilsTests
    {
        IList<int> lst;

        [TestInitialize]
        public void TestInitialize()
        {
            lst = new List<int> { 1, 2, 3, 4, 5 };
        }

        [TestMethod()]
        public void ShiftTest()
        {
            lst.Shift();
            Assert.AreEqual(4,lst.Count);
            Assert.AreEqual(2, lst[0]);
        }

        [TestMethod()]
        public void PopTest()
        {
            lst.Pop();
            Assert.AreEqual(4,lst.Count);
            Assert.AreEqual(4, lst[3]);
        }

        [TestMethod()]
        public void SpliceTest()
        {
            lst.Splice(1, 2);
            Assert.AreEqual(3, lst.Count);
            Assert.AreEqual(1, lst[0]);
            Assert.AreEqual(4, lst[1]);
            Assert.AreEqual(5, lst[2]);
        }  
        
        [TestMethod()]
        public void Splice2Test()
        {
            var arr2 = new Collection<int>() { 1, 2, 3, 4, 5 };
            arr2.Splice(1, 2);
            Assert.AreEqual(3, arr2.Count);
            Assert.AreEqual(1, arr2[0]);
            Assert.AreEqual(4, arr2[1]);
            Assert.AreEqual(5, arr2[2]);
        }

        [TestMethod()]
        public void PushTest()
        {
            lst.Push(0);
            Assert.AreEqual(6, lst.Count);
            Assert.AreEqual(0, lst[5]);
        }

        [TestMethod()]
        public void UnshiftTest()
        {
            lst.Unshift(0);
            Assert.AreEqual(6, lst.Count);
            Assert.AreEqual(0, lst[0]);
        }
    }
}