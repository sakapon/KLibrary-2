using System;
using System.Collections.Generic;
using Keiho;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    /// <summary>
    /// ConvertHelperTest のテスト クラスです。
    /// </summary>
    [TestClass]
    public class ConvertHelperTest
    {
        /// <summary>
        /// 現在のテストの実行についての情報および機能を提供するテスト コンテキストを取得または設定します。
        /// </summary>
        public TestContext TestContext { get; set; }

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
        /// To のテスト
        /// </summary>
        [TestMethod]
        public void ToTest()
        {
            Assert.AreEqual(true, "True".To<bool>());
            Assert.AreEqual(123, "123".To<int>());
            Assert.AreEqual(123.456, "123.456".To<double>());
            Assert.AreEqual(new DateTime(2012, 1, 1, 10, 11, 12), "2012-01-01 10:11:12".To<DateTime>());
            Assert.AreEqual(new TimeSpan(1, 2, 30, 40), "1.02:30:40".To<TimeSpan>());
            Assert.AreEqual(ConsoleColor.Cyan, "Cyan".To<ConsoleColor>());
            Assert.AreEqual(ConsoleColor.Cyan, "11".To<ConsoleColor>());
        }

        /// <summary>
        /// To のテスト
        /// </summary>
        [TestMethod]
        [ExpectedException2(typeof(InvalidCastException), "Null オブジェクトを値型に変換することはできません。")]
        public void ToTest_Int32_Null()
        {
            ((string)null).To<int>();
        }

        /// <summary>
        /// To のテスト
        /// </summary>
        [TestMethod]
        [ExpectedException2(typeof(InvalidCastException), "Null オブジェクトを値型に変換することはできません。")]
        public void ToTest_ConsoleColor_Null()
        {
            ((string)null).To<ConsoleColor>();
        }

        /// <summary>
        /// To のテスト
        /// </summary>
        [TestMethod]
        [ExpectedException2(typeof(FormatException), "入力文字列の形式が正しくありません。")]
        public void ToTest_Int32_FormatError()
        {
            "abc".To<int>();
        }

        /// <summary>
        /// To のテスト
        /// </summary>
        [TestMethod]
        [ExpectedException2(typeof(FormatException), "入力文字列の形式が正しくありません。")]
        public void ToTest_ConsoleColor_FormatError()
        {
            "abc".To<ConsoleColor>();
        }

        /// <summary>
        /// ToHexString のテスト
        /// </summary>
        [TestMethod]
        public void ToHexStringTest()
        {
            byte[] binary = new byte[] { 122, 0, 138, 140, 74, 211, 99, 199, 126, 136, 246, 56, 220, 50, 255, 216 };
            string expected = "7A008A8C4AD363C77E88F638DC32FFD8";

            string actual = binary.ToHexString();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// ToHexString のテスト
        /// </summary>
        [TestMethod]
        public void ToHexStringTest_Lowercase()
        {
            byte[] binary = new byte[] { 122, 0, 138, 140, 74, 211, 99, 199, 126, 136, 246, 56, 220, 50, 255, 216 };
            string expected = "7a008a8c4ad363c77e88f638dc32ffd8";

            string actual = binary.ToHexString(true);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// ToHexString のテスト
        /// </summary>
        [TestMethod]
        public void ToHexStringTest_Size0()
        {
            byte[] binary = new byte[] { };
            string expected = "";

            string actual = binary.ToHexString();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// ToHexString のテスト
        /// </summary>
        [TestMethod]
        [ExpectedException2(typeof(ArgumentNullException))]
        public void ToHexStringTest_Null()
        {
            byte[] binary = null;

            string actual = binary.ToHexString();
        }

        /// <summary>
        /// FromHexString のテスト
        /// </summary>
        [TestMethod]
        public void FromHexStringTest()
        {
            string hexString = "7A008A8C4AD363C77E88F638DC32FFD8";
            byte[] expected = new byte[] { 122, 0, 138, 140, 74, 211, 99, 199, 126, 136, 246, 56, 220, 50, 255, 216 };

            byte[] actual = hexString.FromHexString();

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// FromHexString のテスト
        /// </summary>
        [TestMethod]
        public void FromHexStringTest_Lowercase()
        {
            string hexString = "7a008a8c4ad363c77e88f638dc32ffd8";
            byte[] expected = new byte[] { 122, 0, 138, 140, 74, 211, 99, 199, 126, 136, 246, 56, 220, 50, 255, 216 };

            byte[] actual = hexString.FromHexString();

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// FromHexString のテスト
        /// </summary>
        [TestMethod]
        public void FromHexStringTest_Size0()
        {
            string hexString = "";
            byte[] expected = new byte[] { };

            byte[] actual = hexString.FromHexString();

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// FromHexString のテスト
        /// </summary>
        [TestMethod]
        [ExpectedException2(typeof(ArgumentNullException))]
        public void FromHexStringTest_Null()
        {
            string hexString = null;

            byte[] actual = hexString.FromHexString();
        }

        /// <summary>
        /// FromHexString のテスト
        /// </summary>
        [TestMethod]
        [ExpectedException2(typeof(FormatException), "入力文字列の長さが偶数ではありません。")]
        public void FromHexStringTest_LengthOdd()
        {
            string hexString = "7A008";

            byte[] actual = hexString.FromHexString();
        }

        /// <summary>
        /// FromHexString のテスト
        /// </summary>
        [TestMethod]
        [ExpectedException2(typeof(FormatException), "入力文字列の形式が正しくありません。")]
        public void FromHexStringTest_NotHexadecimal()
        {
            string hexString = "7A00-1";

            byte[] actual = hexString.FromHexString();
        }

        /// <summary>
        /// ToProperties のテスト
        /// </summary>
        [TestMethod]
        public void ToPropertiesTest()
        {
            CollectionAssert.AreEqual(new Dictionary<string, object> { }, true.ToProperties());
            CollectionAssert.AreEqual(new Dictionary<string, object> { { "Length", 3 } }, "123".ToProperties());
            CollectionAssert.AreEqual(new Dictionary<string, object> { { "a", 123 }, { "b", "456" } }, new { a = 123, b = "456" }.ToProperties());
        }

        /// <summary>
        /// ToProperties のテスト
        /// </summary>
        [TestMethod]
        [ExpectedException2(typeof(ArgumentNullException))]
        public void ToPropertiesTest_Null()
        {
            object obj = null;

            obj.ToProperties();
        }
    }
}
