using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Keiho.Threading
{
    public static class TaskUtility
    {
        public static IAsyncResult DoWorkAsync(Action work, Action onCompleted = null)
        {
            if (work == null)
            {
                throw new ArgumentNullException("work");
            }

            return work.BeginInvoke(ar =>
            {
                work.EndInvoke(ar);

                if (onCompleted != null)
                {
                    if (SynchronizationContext.Current == null)
                    {
                        onCompleted();
                    }
                    else
                    {
                        SynchronizationContext.Current.Post(s => onCompleted(), null);
                    }
                }
            }, null);
        }

        public static IAsyncResult DoWorkAsync<T>(Func<T> work, Action<T> onCompleted = null)
        {
            if (work == null)
            {
                throw new ArgumentNullException("work");
            }

            return work.BeginInvoke(ar =>
            {
                T result = work.EndInvoke(ar);

                if (onCompleted != null)
                {
                    if (SynchronizationContext.Current == null)
                    {
                        onCompleted(result);
                    }
                    else
                    {
                        SynchronizationContext.Current.Post(s => onCompleted(result), null);
                    }
                }
            }, null);
        }
    }
}
