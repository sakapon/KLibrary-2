using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Keiho;
using Keiho.Linq;
using Keiho.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Net
{
    /// <summary>
    /// WebDavUtilityTest のテスト クラスです。
    /// </summary>
    [TestClass]
    public class WebDavUtilityTest
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

        private static readonly Uri TargetDirectoryUri = new Uri("https://tempuri.org/Shared/");
        private static readonly Uri TargetFileUri = new Uri("https://tempuri.org/Shared/abc.txt");
        private static readonly NetworkCredential Credentials = new NetworkCredential("user1", "password");

        /// <summary>
        /// Create のテスト
        /// </summary>
        [TestMethod]
        public void CreateTest()
        {
            Uri address = TargetFileUri;
            ICredentials credentials = Credentials;
            byte[] content = Encoding.UTF8.GetBytes("abcde");

            WebDavUtility.Create(address, content, credentials);
        }

        /// <summary>
        /// Read のテスト
        /// </summary>
        [TestMethod]
        public void ReadTest()
        {
            Uri address = TargetFileUri;
            ICredentials credentials = Credentials;
            byte[] expected = Encoding.UTF8.GetBytes("abcde");

            byte[] actual = WebDavUtility.Read(address, credentials);
            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Update のテスト
        /// </summary>
        [TestMethod]
        public void UpdateTest()
        {
            Uri address = TargetFileUri;
            ICredentials credentials = Credentials;
            byte[] content = Encoding.UTF8.GetBytes("abcdefghij");

            WebDavUtility.Update(address, content, credentials);
        }

        /// <summary>
        /// Delete のテスト
        /// </summary>
        [TestMethod]
        public void DeleteTest()
        {
            Uri address = TargetFileUri;
            ICredentials credentials = Credentials;

            WebDavUtility.Delete(address, credentials);
        }

        /// <summary>
        /// OpenCreate のテスト
        /// </summary>
        [TestMethod]
        public void OpenCreateTest()
        {
            Uri address = TargetFileUri;
            ICredentials credentials = Credentials;
            byte[] content = Encoding.UTF8.GetBytes("abcde");

            using (var input = new MemoryStream(content))
            using (var output = WebDavUtility.OpenCreate(address, credentials))
            {
                input.CopyTo(output);
            }
        }

        /// <summary>
        /// OpenRead のテスト
        /// </summary>
        [TestMethod]
        public void OpenReadTest()
        {
            Uri address = TargetFileUri;
            ICredentials credentials = Credentials;
            byte[] expected = Encoding.UTF8.GetBytes("abcde");

            using (var input = WebDavUtility.OpenRead(address, credentials))
            using (var output = new MemoryStream())
            {
                input.CopyTo(output);
                byte[] actual = output.ToArray();

                CollectionAssert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// OpenUpdate のテスト
        /// </summary>
        [TestMethod]
        public void OpenUpdateTest()
        {
            Uri address = TargetFileUri;
            ICredentials credentials = Credentials;
            byte[] content = Encoding.UTF8.GetBytes("abcdefghij");

            using (var input = new MemoryStream(content))
            using (var output = WebDavUtility.OpenUpdate(address, credentials))
            {
                input.CopyTo(output);
            }
        }

        /// <summary>
        /// GetChildren のテスト
        /// </summary>
        [TestMethod]
        public void GetChildrenTest()
        {
            Uri address = TargetDirectoryUri;
            ICredentials credentials = Credentials;

            var actual = WebDavUtility.GetChildren(address, Credentials);

            actual.ForEachExecute(e => Console.WriteLine(e.Uri.GetName()));
            Assert.Inconclusive("出力結果を目視で確認してください。");
        }

        /// <summary>
        /// GetProperties のテスト
        /// </summary>
        [TestMethod]
        public void GetPropertiesTest()
        {
            Uri address = TargetDirectoryUri;
            int depth = 1;
            ICredentials credentials = Credentials;

            var actual = WebDavUtility.GetProperties(address, depth, Credentials);

            actual.ForEachExecute(e => Console.WriteLine(e.Uri.GetName()));
            Assert.Inconclusive("出力結果を目視で確認してください。");
        }
    }
}
