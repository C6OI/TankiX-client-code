using System;
using System.Reflection;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class MultipleContextNodesNotSupportedException : Exception {
        public MultipleContextNodesNotSupportedException(MethodInfo methodInfo)
            : base("Method: " + methodInfo) { }
    }
}