using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Keiho.Collections
{
    public class DelegatableComparer<T> : IEqualityComparer<T>
    {
        private Func<T, int> getHashCodeDelegate;

        public DelegatableComparer(Func<T, int> getHashCode)
        {
            getHashCodeDelegate = getHashCode;
        }

        public bool Equals(T x, T y)
        {
            return GetHashCode(x) == GetHashCode(y);
        }

        public int GetHashCode(T obj)
        {
            return getHashCodeDelegate(obj);
        }
    }
}
