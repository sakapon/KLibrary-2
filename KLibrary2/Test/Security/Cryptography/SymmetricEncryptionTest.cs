using System;
using System.IO;
using System.Text;
using Keiho.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Security.Cryptography
{
    /// <summary>
    /// SymmetricEncryptionTest のテスト クラスです。
    /// </summary>
    [TestClass]
    public class SymmetricEncryptionTest
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
        /// Encrypt, Decrypt のテスト
        /// </summary>
        [TestMethod]
        public void EncryptDecryptTest_Text()
        {
            string decryptionKey = "7A008A8C4AD363C77E88F638DC32FFD8";
            string algorithmName = "Rijndael";

            string text = "これは、SymmetricEncryption クラスのテストです。";
            string expectedEncrypted = "22C7WXT5anDnN46oJrjSzwb/UpREgOQ6bdmpe+WstZqC7GUEBcKoYIuwztHXsdw5yFW+ixhHG8/PCXP4JhH2llNPk6RcEarVbSwIbLu+0Wg=";

            string actual1 = SymmetricEncryption.Encrypt(text, decryptionKey, algorithmName);
            Console.WriteLine(actual1);
            Assert.AreEqual(expectedEncrypted, actual1);

            string actual2 = SymmetricEncryption.Decrypt(actual1, decryptionKey, algorithmName);
            Console.WriteLine(actual2);
            Assert.AreEqual(text, actual2);
        }

        /// <summary>
        /// Encrypt, Decrypt のテスト
        /// </summary>
        [TestMethod]
        public void EncryptDecryptTest_Stream()
        {
            string decryptionKey = "7A008A8C4AD363C77E88F638DC32FFD8";
            string algorithmName = "Rijndael";

            string sourceFileName = "Source.txt";
            string encryptedFileName = "Encrypted.enc";
            string decryptedFileName = "Decrypted.txt";

            string text = "これは、SymmetricEncryption クラスのテストです。";
            File.WriteAllText(sourceFileName, text, Encoding.UTF8);

            using (Stream input = File.OpenRead(sourceFileName))
            using (Stream output = File.OpenWrite(encryptedFileName))
            {
                SymmetricEncryption.Encrypt(input, output, decryptionKey, algorithmName);
            }

            using (Stream input = File.OpenRead(encryptedFileName))
            using (Stream output = File.OpenWrite(decryptedFileName))
            {
                SymmetricEncryption.Decrypt(input, output, decryptionKey, algorithmName);
            }

            CollectionAssert.AreEqual(File.ReadAllBytes(sourceFileName), File.ReadAllBytes(decryptedFileName));
        }
    }
}
