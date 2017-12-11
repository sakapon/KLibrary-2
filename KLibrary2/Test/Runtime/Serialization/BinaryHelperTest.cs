using System;
using System.Collections.Generic;
using System.IO;
using Keiho.Runtime.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Runtime.Serialization
{
    /// <summary>
    /// BinaryHelperTest のテスト クラスです。
    /// </summary>
    [TestClass]
    public class BinaryHelperTest
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
        /// ToBinary, FromBinary のテスト
        /// </summary>
        [TestMethod]
        public void ToBinaryFromBinaryTest_Binary()
        {
            var obj = new KeyValuePair<int, string>(999, "これは、BinaryHelper クラスのテストです。");

            var actual1 = obj.ToBinary();
            var actual2 = actual1.FromBinary<KeyValuePair<int, string>>();

            Assert.AreEqual(obj, actual2);
        }

        /// <summary>
        /// ToBinary, FromBinary のテスト
        /// </summary>
        [TestMethod]
        public void ToBinaryFromBinaryTest_Stream()
        {
            var obj = new KeyValuePair<int, string>(999, "これは、BinaryHelper クラスのテストです。");
            var stream = new MemoryStream();

            obj.ToBinary(stream);
            stream.Position = 0;
            var actual2 = stream.FromBinary<KeyValuePair<int, string>>();

            Assert.AreEqual(obj, actual2);
        }
    }
}
