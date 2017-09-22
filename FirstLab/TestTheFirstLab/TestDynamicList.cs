using Microsoft.VisualStudio.TestTools.UnitTesting;
using FirstLab;
using System;

namespace TestTheFirstLab
{
    [TestClass]
    public class TestDynamicList
    {
        private const int ELEMENT_COUNT = 10;

        private DynamicList<int> list;
        private DynamicList<TestClass> _list;

        public void Init()
        {
            list = new DynamicList<int>();
            _list = new DynamicList<TestClass>();
        }

        [TestMethod]
        public void TestObjectElement()
        {
            Init();
            TestClass test = new TestClass(10);
            _list.Add(test);
            Assert.AreEqual(_list[0], test);
        }

        [TestMethod]
        public void TestAddOneElement()
        {
            Init();
            list.Add(3);
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(3, list[0]);
        }

        [TestMethod]
        public void TestAddManyElement()
        {
            Init();
            for (int i = 0; i < ELEMENT_COUNT; i++)
                list.Add(i);
            Assert.AreEqual(ELEMENT_COUNT, list.Count);
            for (int i = 0; i < ELEMENT_COUNT; i++)
                Assert.AreEqual(i, list[i]);
        }

        [TestMethod]
        public void TestRemoveElement()
        {
            Init();
            for (int i = 0; i < ELEMENT_COUNT; i++)
                list.Add(i);
            list.Remove(0);
            list.Remove(ELEMENT_COUNT + 1);
            
            Assert.AreEqual(ELEMENT_COUNT - 1,  list.Count);
            for (int i = 0; i < ELEMENT_COUNT - 1; i++)
              Assert.AreNotEqual(0, list[i]);           
        }

        [TestMethod]
        public void TestRemoveManyElements()
        {
            Init();
            for (int i = 0; i < ELEMENT_COUNT; i++)
                list.Add(i);

            Assert.AreEqual(ELEMENT_COUNT, list.Count);
            Assert.AreEqual(13, list.Capacity);

            int listCapacity = list.Capacity;
            for (int i = 0; i < listCapacity / 2 + 2; i++)
                list.Remove(i);

            Assert.AreEqual(6, list.Capacity);

            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(8, list[0]);
            Assert.AreEqual(9, list[1]);
        }

        [TestMethod]
        public void TestForeach()
        {
            Init();
            for (int i = 0; i < ELEMENT_COUNT; i++)
                list.Add(i);
            Type type = typeof(int);
            foreach(var item in list)
            {
                Assert.IsNotNull(item);
                Assert.IsInstanceOfType(item, type);
            }
        }

        [TestMethod]
        public void TestRemoveAt()
        {
            Init();
            for (int i = 0; i < ELEMENT_COUNT; i++)
                list.Add(i);
            list.RemoveAt(0);

            Assert.AreEqual(ELEMENT_COUNT - 1, list.Count);
            for (int i = 0; i < ELEMENT_COUNT - 1; i++)
                Assert.AreNotEqual(0, list[i]);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TestRemoveAtException()
        {
            Init();
            for (int i = 0; i < ELEMENT_COUNT; i++)
                list.Add(i);
            list.RemoveAt(11);
        }

        [TestMethod]
        public void TestClear()
        {
            Init();
            for (int i = 0; i < ELEMENT_COUNT; i++)
                list.Add(i);
            list.Clear();
            Assert.AreEqual(0, list.Count);
        }



        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TestGetSetException()
        {
            Init();
            for (int i = 0; i < ELEMENT_COUNT; i++)
                list.Add(i);
            list[ELEMENT_COUNT + 1] = 1;
        }

    }
}
