using System;
using Keiho.Net.Mail;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Net.Mail
{
    /// <summary>
    /// SmtpUtilityTest のテスト クラスです。
    /// </summary>
    [TestClass]
    public class SmtpUtilityTest
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
        /// Send のテスト
        /// </summary>
        [TestMethod]
        public void SendTest()
        {
            string from = "from@gmail.com";
            string[] to = new[] { "to@gmail.com" };
            string[] cc = new[] { "cc@gmail.com" };
            string subject = "SmtpUtility のテスト";
            string body = "SmtpUtility のテストです。\r\n\r\n3 行目です。\r\n";

            SmtpUtility.Send(from, to, cc, subject, body);

            Assert.Inconclusive("電子メールを受信したことを確認してください。");
        }
    }
}
