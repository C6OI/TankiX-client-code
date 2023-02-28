using System;
using System.Reflection;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class JoinFirstNodeArgumentException : Exception {
        public JoinFirstNodeArgumentException(MethodInfo method, HandlerArgument handlerArgument)
            : base(string.Format("Join-node can''t be first node-argument of method\n[method={0},\nhandlerArgument={1}]", method, handlerArgument)) { }
    }
}