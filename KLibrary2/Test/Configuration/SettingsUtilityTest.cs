using System;
using Keiho.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Configuration
{
    /// <summary>
    /// SettingsUtilityTest のテスト クラスです。
    /// </summary>
    [TestClass]
    public class SettingsUtilityTest
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
        /// GetAppSetting のテスト
        /// </summary>
        [TestMethod]
        public void GetAppSettingTest()
        {
            Assert.AreEqual(true, SettingsUtility.GetAppSetting("IsEnabled", false));
            Assert.AreEqual(99, SettingsUtility.GetAppSetting("Count", 30));
            Assert.AreEqual(new DateTime(2012, 1, 1, 10, 20, 30), SettingsUtility.GetAppSetting("Since", new DateTime(2000, 1, 1)));
            Assert.AreEqual(TimeSpan.FromSeconds(90), SettingsUtility.GetAppSetting("Timeout", TimeSpan.FromSeconds(30)));
            Assert.AreEqual(DayOfWeek.Monday, SettingsUtility.GetAppSetting("DayOfWeek", DayOfWeek.Sunday));

            Assert.AreEqual(false, SettingsUtility.GetAppSetting("Nokey", false));
            Assert.AreEqual(123, SettingsUtility.GetAppSetting("Nokey", 123));
        }

        /// <summary>
        /// GetSectionSetting のテスト
        /// </summary>
        [TestMethod]
        public void GetSectionSettingTest()
        {
            Assert.AreEqual(true, SettingsUtility.GetSectionSetting("keiho.configuration/singleTag1", "IsEnabled", false));
            Assert.AreEqual(99, SettingsUtility.GetSectionSetting("keiho.configuration/singleTag1", "Count", 30));
            Assert.AreEqual(123, SettingsUtility.GetSectionSetting("keiho.configuration/singleTag1", "NoKey", 123));

            Assert.AreEqual(3, SettingsUtility.GetSectionSetting("keiho.configuration/dictionary1", "Key3", 30));
            Assert.AreEqual(123, SettingsUtility.GetSectionSetting("keiho.configuration/dictionary1", "NoKey", 123));
        }
    }
}
