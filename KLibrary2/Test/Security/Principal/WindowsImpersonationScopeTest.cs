using System;
using System.ComponentModel;
using System.Security.Principal;
using Keiho.Security.Principal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Security.Principal
{
    /// <summary>
    /// WindowsImpersonationScopeTest のテスト クラスです。
    /// </summary>
    [TestClass]
    public class WindowsImpersonationScopeTest
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
        /// WindowsImpersonationScope コンストラクター のテスト
        /// </summary>
        [TestMethod]
        public void ConstructorTest()
        {
            string username = "user1";
            string password = "password";
            string domain = "";

            var beforeId = WindowsIdentity.GetCurrent();
            Assert.AreEqual(TokenImpersonationLevel.None, beforeId.ImpersonationLevel);

            using (var scope = new WindowsImpersonationScope(username, password, domain))
            {
                var id = WindowsIdentity.GetCurrent();
                Assert.AreEqual(TokenImpersonationLevel.Impersonation, id.ImpersonationLevel);
                Assert.AreEqual(string.Format(@"{0}\{1}", string.IsNullOrEmpty(domain) ? Environment.MachineName : domain, username), id.Name, true);
            }

            var afterId = WindowsIdentity.GetCurrent();
            Assert.AreEqual(TokenImpersonationLevel.None, afterId.ImpersonationLevel);
            Assert.AreEqual(beforeId.Name, afterId.Name);
        }

        /// <summary>
        /// WindowsImpersonationScope コンストラクター のテスト
        /// </summary>
        [TestMethod]
        [ExpectedException2(typeof(Win32Exception), "ログオン失敗: ユーザー名を認識できないか、またはパスワードが間違っています。")]
        public void ConstructorTest_LogonError()
        {
            string username = "user1";
            string password = "p";
            string domain = "";

            using (var scope = new WindowsImpersonationScope(username, password, domain))
            {
            }
        }

        /// <summary>
        /// Dispose のテスト
        /// </summary>
        [TestMethod]
        public void DisposeTest()
        {
            string username = "user1";
            string password = "password";
            string domain = "";

            using (var scope = new WindowsImpersonationScope(username, password, domain))
            {
                // Dispose メソッドを複数回呼び出します。
                scope.Dispose();
                scope.Dispose();
            }
        }
    }
}
