using System;
using System.Text.RegularExpressions;
using Keiho;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    /// <summary>
    /// RandomUtilityTest のテスト クラスです。
    /// </summary>
    [TestClass]
    public class RandomUtilityTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///現在のテストの実行についての情報および機能を
        ///提供するテスト コンテキストを取得または設定します。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 追加のテスト属性
        // 
        //テストを作成するときに、次の追加属性を使用することができます:
        //
        //クラスの最初のテストを実行する前にコードを実行するには、ClassInitialize を使用
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //クラスのすべてのテストを実行した後にコードを実行するには、ClassCleanup を使用
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //各テストを実行する前にコードを実行するには、TestInitialize を使用
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //各テストを実行した後にコードを実行するには、TestCleanup を使用
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        /// GenerateBytes のテスト
        /// </summary>
        [TestMethod]
        public void GenerateBytesTest()
        {
            int size = 16;

            byte[] actual = RandomUtility.GenerateBytes(size);

            Console.WriteLine(string.Join(", ", actual));
            Assert.AreEqual(size, actual.Length);
        }

        /// <summary>
        /// GenerateBytes のテスト
        /// </summary>
        [TestMethod]
        public void GenerateBytesTest_Size0()
        {
            int size = 0;

            byte[] actual = RandomUtility.GenerateBytes(size);

            Console.WriteLine(string.Join(", ", actual));
            Assert.AreEqual(size, actual.Length);
        }

        /// <summary>
        /// GenerateBytes のテスト
        /// </summary>
        [TestMethod]
        [ExpectedException2(typeof(ArgumentOutOfRangeException))]
        public void GenerateBytesTest_SizeNegative()
        {
            int size = -1;

            byte[] actual = RandomUtility.GenerateBytes(size);
        }

        /// <summary>
        /// GenerateBase64String のテスト
        /// </summary>
        [TestMethod]
        public void GenerateBase64StringTest()
        {
            int size = 16;

            string actual = RandomUtility.GenerateBase64String(size);

            Console.WriteLine(actual);
            var binary = Convert.FromBase64String(actual);
            Assert.AreEqual(size, binary.Length);
        }

        /// <summary>
        /// GenerateBase64String のテスト
        /// </summary>
        [TestMethod]
        public void GenerateBase64StringTest_Size0()
        {
            int size = 0;

            string actual = RandomUtility.GenerateBase64String(size);

            Console.WriteLine(actual);
            var binary = Convert.FromBase64String(actual);
            Assert.AreEqual(size, binary.Length);
        }

        /// <summary>
        /// GenerateBase64String のテスト
        /// </summary>
        [TestMethod]
        [ExpectedException2(typeof(ArgumentOutOfRangeException))]
        public void GenerateBase64StringTest_SizeNegative()
        {
            string actual = RandomUtility.GenerateBase64String(-1);
        }

        /// <summary>
        /// GenerateHexString のテスト
        /// </summary>
        [TestMethod]
        public void GenerateHexStringTest()
        {
            int size = 16;
            bool lowercase = false;

            string actual = RandomUtility.GenerateHexString(size, lowercase);

            Console.WriteLine(actual);
            Assert.AreEqual(2 * size, actual.Length);
            Assert.IsTrue(Regex.IsMatch(actual, "^[0-9A-F]*$"));
        }

        /// <summary>
        /// GenerateHexString のテスト
        /// </summary>
        [TestMethod]
        public void GenerateHexStringTest_Lowercase()
        {
            int size = 16;
            bool lowercase = true;

            string actual = RandomUtility.GenerateHexString(size, lowercase);

            Console.WriteLine(actual);
            Assert.AreEqual(2 * size, actual.Length);
            Assert.IsTrue(Regex.IsMatch(actual, "^[0-9a-f]*$"));
        }

        /// <summary>
        /// GenerateHexString のテスト
        /// </summary>
        [TestMethod]
        public void GenerateHexStringTest_Size0()
        {
            int size = 0;

            string actual = RandomUtility.GenerateHexString(size);

            Console.WriteLine(actual);
            Assert.AreEqual(2 * size, actual.Length);
        }

        /// <summary>
        /// GenerateHexString のテスト
        /// </summary>
        [TestMethod]
        [ExpectedException2(typeof(ArgumentOutOfRangeException))]
        public void GenerateHexStringTest_SizeNegative()
        {
            string actual = RandomUtility.GenerateHexString(-1);
        }
    }
}
