using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Keiho.Threading
{
    public class TaskWorker
    {
        private Action task;
        private List<EventAction> callbackActions;

        public TaskWorker(Action task)
        {
            this.task = task;
            callbackActions = new List<EventAction>();
        }

        public TaskWorker OnCompleted(Action action)
        {
            callbackActions.Add(new EventAction { ResultType = ResultType.Completed, Action = action });
            return this;
        }

        public TaskWorker OnSuccess(Action action)
        {
            callbackActions.Add(new EventAction { ResultType = ResultType.Success, Action = action });
            return this;
        }

        public TaskWorker OnError(Action<Exception> action)
        {
            callbackActions.Add(new EventAction { ResultType = ResultType.Error, Action = action });
            return this;
        }

        public TaskWorker OnCanceled(Action action)
        {
            throw new NotImplementedException();
        }

        public TaskWorker ContinueWith(Action action)
        {
            throw new NotImplementedException();
        }

        public void RunSync()
        {
            throw new NotImplementedException();
        }

        public TaskWorker RunAsync()
        {
            throw new NotImplementedException();
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        [Flags]
        private enum ResultType
        {
            Canceled = 0,
            Completed = 1,
            Success = 2,
            Error = 4,
        }

        private class EventAction
        {
            public ResultType ResultType { get; set; }
            public Delegate Action { get; set; }
        }
    }
}
