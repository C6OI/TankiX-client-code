using System;
using System.Reflection;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class JoinToCollectionException : Exception {
        public JoinToCollectionException(MethodInfo method, HandlerArgument handlerArgument)
            : base(string.Format("Join-node can''t be joined by collection\n[method={0},\nhandlerArgument={1}]",
                method,
                handlerArgument)) { }
    }
}