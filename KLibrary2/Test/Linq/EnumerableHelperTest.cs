﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Keiho.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Linq
{
    /// <summary>
    /// EnumerableHelperTest のテスト クラスです。すべての
    /// EnumerableHelperTest 単体テストをここに含めます
    /// </summary>
    [TestClass]
    public class EnumerableHelperTest
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
        /// GetRecursively のテスト
        /// </summary>
        [TestMethod]
        public void GetRecursivelyTest_Directories()
        {
            EnumerableHelper.GetRecursively(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), Directory.GetDirectories)
                .ForEachExecute(Console.WriteLine);
        }

        /// <summary>
        /// GetRecursively のテスト
        /// </summary>
        [TestMethod]
        public void GetRecursivelyTest_Files()
        {
            EnumerableHelper.GetRecursively(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), Directory.GetDirectories, Directory.GetFiles)
                .ForEachExecute(Console.WriteLine);
        }
    }
}
