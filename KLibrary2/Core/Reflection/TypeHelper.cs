using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Keiho.Linq;

namespace Keiho.Reflection
{
    public static class TypeHelper
    {
        public static TResult Clone<TResult>(this object obj) where TResult : new()
        {
            var result = new TResult();

            var props1 = TypeDescriptor.GetProperties(obj).Cast<PropertyDescriptor>().ToArray();
            var props2 = TypeDescriptor.GetProperties(result).Cast<PropertyDescriptor>().ToArray();

            props2
                .Where(p2 => !p2.IsReadOnly)
                .Select(p2 => new { Prop1 = props1.FirstOrDefault(p1 => p1.Name == p2.Name), Prop2 = p2 })
                .Where(p => p.Prop1 != null)
                .Where(p => p.Prop1.PropertyType == p.Prop2.PropertyType)
                .ForEachExecute(p => p.Prop2.SetValue(result, p.Prop1.GetValue(obj)));

            return result;
        }

        public static void CopyTo(this object input, ref object output)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<Type> GetBaseTypes<T>()
        {
            return typeof(T).GetBaseTypes();
        }

        public static IEnumerable<Type> GetBaseTypes(this object obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            return obj.GetType().GetBaseTypes();
        }

        public static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            Type current = type;
            do
            {
                yield return current;
            }
            while ((current = current.BaseType) != null);
        }
    }
}
