using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Keiho.Runtime.Remoting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Runtime.Remoting
{
    /// <summary>
    /// ProxyUtilityTest のテスト クラスです。
    /// </summary>
    [TestClass]
    public class ProxyUtilityTest
    {
        /// <summary>
        /// 現在のテストの実行についての情報および機能を提供するテスト コンテキストを取得または設定します。
        /// </summary>
        public TestContext TestContext { get; set; }

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
        /// Create のテスト
        /// </summary>
        [TestMethod]
        public void CreateTest()
        {
            var actual = ProxyUtility.Create<Object1, LogProxy<Object1>>();
            actual.Method1();
        }

        /// <summary>
        /// CreateFake のテスト
        /// </summary>
        [TestMethod]
        public void CreateFakeTest()
        {
            var actual = ProxyUtility.CreateFake<IFormattable, FormatProxy>();
            var text = actual.ToString("ABC: {0}", null);
        }

        /// <summary>
        /// CreateFake のテスト
        /// </summary>
        [TestMethod]
        [ExpectedException2(typeof(InvalidOperationException))]
        public void CreateFakeTest_NotImplemented()
        {
            var actual = ProxyUtility.CreateFake<Object1, ProxyBase<Object1>>();
            actual.Method1();
        }

        /// <summary>
        /// ToProxy のテスト
        /// </summary>
        [TestMethod]
        public void ToProxyTest()
        {
            var actual = ProxyUtility.ToProxy<IComparable, ProxyBase<IComparable>>("Text");
            int value = actual.CompareTo("abc");
        }

        /// <summary>
        /// ProxyTypeAttribute のテスト
        /// </summary>
        [TestMethod]
        public void ProxyTypeAttributeTest()
        {
            var actual = new Object2();
            actual.Property1 = 100;
            int i = actual.Property1;
            actual.Method1();
            actual.Method2();
        }

        private class Object1 : MarshalByRefObject
        {
            public void Method1()
            {
                Debug.WriteLine("Invoke", "Method1");
            }
        }

        [ProxyType(typeof(LogProxy<Object2>))]
        private class Object2 : ContextBoundObject
        {
            public int Property1 { get; set; }

            [Log(BeforeInvoke = true, AfterInvoke = true)]
            public void Method1()
            {
                Debug.WriteLine("Invoke", "Method1");
            }

            [Log(BeforeInvoke = true)]
            public void Method2()
            {
                Debug.WriteLine("Invoke", "Method2");
            }
        }

        private class LogProxy<T> : ProxyBase<T> where T : class
        {
            protected override IMethodReturnMessage InvokeMethod(IMethodCallMessage methodCall)
            {
                var logAttribute = methodCall.MethodBase
                    .GetCustomAttributes(true)
                    .OfType<LogAttribute>()
                    .SingleOrDefault();

                if (logAttribute != null && logAttribute.BeforeInvoke)
                {
                    Debug.WriteLine("Start", methodCall.MethodName);
                }

                var methodReturn = base.InvokeMethod(methodCall);

                if (logAttribute != null && logAttribute.AfterInvoke)
                {
                    Debug.WriteLine("End", methodCall.MethodName);
                }

                return methodReturn;
            }
        }

        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
        private sealed class LogAttribute : Attribute
        {
            public bool BeforeInvoke { get; set; }
            public bool AfterInvoke { get; set; }
        }

        private class FormatProxy : ProxyBase<IFormattable>
        {
            protected override IMethodReturnMessage InvokeMethod(IMethodCallMessage methodCall)
            {
                switch (methodCall.MethodName)
                {
                    case "ToString":
                        return CreateNormalMethodReturn(string.Format((string)methodCall.Args[0], this), methodCall);
                    default:
                        return base.InvokeMethod(methodCall);
                }
            }
        }
    }
}
