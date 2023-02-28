using System;
using System.Threading;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class MultiThreadedExecutor : Executor {
        public void Execute(Action action) {
            new Thread(action.Invoke).Start();
        }
    }
}