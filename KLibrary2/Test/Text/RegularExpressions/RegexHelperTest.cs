using System;
using System.Collections.Generic;
using Keiho.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Text.RegularExpressions
{
    /// <summary>
    /// RegexHelperTest のテスト クラスです。
    /// </summary>
    [TestClass]
    public class RegexHelperTest
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
        /// Match のテスト
        /// </summary>
        [TestMethod]
        public void MatchTest()
        {
            Assert.AreEqual("aaa<table><tr></tr></table>aaa", "aaa<table><tr></tr></table>aaa".Match("^.*(<table>.*</table>).*$", 0));
            Assert.AreEqual("<table><tr></tr></table>", "aaa<table><tr></tr></table>aaa".Match("^.*(<table>.*</table>).*$", 1));
            Assert.AreEqual("content1\r\ncontent2", "aaa#begin#content1\r\ncontent2#end#aaa".Match("^.*#begin#(.*)#end#.*$", 1));
            Assert.AreEqual(null, "a".Match("^.*b.*$", 0));

            Assert.AreEqual("1", "a<table><tr>1</tr><tr>2</tr><tr>3</tr></table>a".Match("<tr( [^>]*)?>([^<]*)</tr>", 2));
        }

        /// <summary>
        /// Match のテスト
        /// </summary>
        [TestMethod]
        [ExpectedException2(typeof(ArgumentOutOfRangeException))]
        public void MatchTest_GroupIndex_Big()
        {
            "aaa#begin#content1\r\ncontent2#end#aaa".Match("^.*#begin#(.*)#end#.*$", 2);
        }

        /// <summary>
        /// Match のテスト
        /// </summary>
        [TestMethod]
        public void MatchTest2()
        {
            CollectionAssert.AreEqual(new[] { "1", "2", "3" }, "aaa<a>1</a><b>2</b><c>3</c>aaa".Match("^.*<a>(.*)</a><b>(.*)</b><c>(.*)</c>.*$", 1, 2, 3));
        }

        /// <summary>
        /// Matches のテスト
        /// </summary>
        [TestMethod]
        public void MatchesTest()
        {
            CollectionAssert.AreEqual(new[] { "1", "2", "3" }, "a<table><tr>1</tr><tr>2</tr><tr>3</tr></table>a".Matches("<tr( [^>]*)?>([^<]*)</tr>", 2));
        }

        /// <summary>
        ///ReplaceMatch のテスト
        ///</summary>
        [TestMethod]
        public void ReplaceMatchTest()
        {
            Assert.AreEqual("aaa@@@aaa", "aaa<a 123 /><a 456 /><a 789 />aaa".ReplaceMatch("<a( [^>]*)?/>", "@"));
            Assert.AreEqual("10[%] (^[_]^)/ [[]0]", "10% (^_^)/ [0]".ReplaceMatch(@"[%_\[]", "[$0]"));
            Assert.AreEqual("a[1][2][3]a", "a<1><2><3>a".ReplaceMatch(@"<(\d*)>", "[$1]"));
        }
    }
}
