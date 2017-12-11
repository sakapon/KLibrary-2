using System;
using Keiho;
using Keiho.Net;
using Keiho.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Net
{
    /// <summary>
    /// HttpWebUtilityTest のテスト クラスです。
    /// </summary>
    [TestClass]
    public class HttpWebUtilityTest
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
        /// DownloadString のテスト
        /// </summary>
        [TestMethod]
        public void DownloadStringTest()
        {
            string targetUri = "http://saka-pon.net/";

            var result = TaskUtility.DoWorkAsync(
                () => HttpWebUtility.DownloadString(targetUri),
                s => Console.WriteLine(s)
            );

            result.AsyncWaitHandle.WaitOne();

            Assert.Inconclusive("出力結果を目視で確認してください。");
        }

        /// <summary>
        /// DownloadString のテスト
        /// </summary>
        [TestMethod]
        public void DownloadStringTest_Headers()
        {
            string targetUri = "http://www.bing.com/";

            Console.WriteLine("Windows 7, IE 9:");
            Console.WriteLine(HttpWebUtility.DownloadString(targetUri, new { UserAgent = SharedObjects.UserAgents.Windows7_IE9 }));
            Console.WriteLine();
            Console.WriteLine("iOS 5, Safari 5.1:");
            Console.WriteLine(HttpWebUtility.DownloadString(targetUri, new { UserAgent = SharedObjects.UserAgents.IOS5_Safari51 }));

            Assert.Inconclusive("出力結果を目視で確認してください。");
        }

        /// <summary>
        /// DownloadStringAsync のテスト
        /// </summary>
        [TestMethod]
        public void DownloadStringAsyncTest()
        {
            string targetUri = string.Empty; // TODO: 適切な値に初期化してください
            Action<string> onCompleted = null; // TODO: 適切な値に初期化してください
            Action<Exception> onError = null; // TODO: 適切な値に初期化してください
            HttpWebUtility.DownloadStringAsync(targetUri, onCompleted, onError);
            Assert.Inconclusive("値を返さないメソッドは確認できません。");
        }
    }
}
