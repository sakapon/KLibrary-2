using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class ExpectedException2Attribute : ExpectedExceptionBaseAttribute
    {
        public Type ExceptionType { get; private set; }
        public string Message { get; private set; }
        public bool AllowDerivedTypes { get; set; }

        public ExpectedException2Attribute(Type exceptionType)
            : this(exceptionType, null)
        {
        }

        public ExpectedException2Attribute(Type exceptionType, string message)
        {
            if (exceptionType == null)
            {
                throw new ArgumentNullException("exceptionType");
            }
            if (!typeof(Exception).IsAssignableFrom(exceptionType))
            {
                throw new ArgumentException("予期される例外の型は System.Exception または System.Exception の派生型である必要があります。", "exceptionType");
            }

            ExceptionType = exceptionType;
            Message = message;
        }

        protected override void Verify(Exception exception)
        {
            Type actualType = exception.GetType();

            if (AllowDerivedTypes)
            {
                if (!ExceptionType.IsAssignableFrom(actualType))
                {
                    base.RethrowIfAssertException(exception);
                    throw new Exception(string.Format("例外 {0} またはその派生型が予期されていましたが、例外 {1} がスローされました。\r\n{2}", ExceptionType.FullName, actualType.FullName, exception));
                }
            }
            else
            {
                if (actualType != ExceptionType)
                {
                    base.RethrowIfAssertException(exception);
                    throw new Exception(string.Format("例外 {0} が予期されていましたが、例外 {1} がスローされました。\r\n{2}", ExceptionType.FullName, actualType.FullName, exception));
                }
            }

            if (Message != null && exception.Message != Message)
            {
                base.RethrowIfAssertException(exception);
                throw new Exception(string.Format("例外メッセージ <{0}> が予期されていました。\r\n{1}", Message, exception));
            }
        }
    }
}
