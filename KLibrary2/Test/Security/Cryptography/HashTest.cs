using System;
using Keiho.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Security.Cryptography
{
    /// <summary>
    /// HashTest のテスト クラスです。
    /// </summary>
    [TestClass]
    public class HashTest
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
        /// Compute のテスト
        /// </summary>
        [TestMethod]
        public void ComputeTest()
        {
            string text = "P@ssw0rd";
            string salt = "jkQLkPYg2TNhyGJqLXdylA==";
            string algorithmName = "HMACSHA256";
            string expected = "Gx4eFLaG+nC8ZHosevcyDvCIQIfCuvTdSyosyQNTVFc=";

            string actual = Hash.Compute(text, salt, algorithmName);

            Assert.AreEqual(expected, actual);
        }
    }
}
