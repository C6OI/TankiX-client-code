using System;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public interface Executor {
        void Execute(Action action);
    }
}