using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Keiho.Reflection
{
    public static class MemberHelper
    {
        /// <summary>
        /// オブジェクトから、指定されたプロパティの値を取得します。
        /// </summary>
        /// <typeparam name="TResult">プロパティの値の型。</typeparam>
        /// <param name="obj">任意のオブジェクト。</param>
        /// <param name="propertyName">プロパティの名前。</param>
        /// <param name="declaringType">プロパティが宣言されている型。<see langword="null"/> の場合、パブリック プロパティのみが対象となります。</param>
        /// <returns>プロパティの値。</returns>
        public static TResult GetPropertyValue<TResult>(this object obj, string propertyName, Type declaringType = null)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (propertyName == null) throw new ArgumentNullException("propertyName");
            if (propertyName.Length == 0) throw new ArgumentException("値を空にすることはできません。", "propertyName");

            if (declaringType == null)
            {
                var descriptor = TypeDescriptor.GetProperties(obj)
                    .Cast<PropertyDescriptor>()
                    .SingleOrDefault(p => p.Name == propertyName);

                if (descriptor == null) throw new ArgumentException("指定された名前のプロパティが存在しません。", "propertyName");

                return (TResult)descriptor.GetValue(obj);
            }
            else
            {
                var info = declaringType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty);

                if (info == null) throw new ArgumentException("指定された名前のプロパティが存在しません。", "propertyName");

                return (TResult)info.GetValue(obj, null);
            }
        }
    }
}
