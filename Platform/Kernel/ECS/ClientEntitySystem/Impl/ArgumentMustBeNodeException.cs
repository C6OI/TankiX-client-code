using System;
using System.Reflection;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class ArgumentMustBeNodeException : Exception {
        public ArgumentMustBeNodeException(MethodInfo method, ParameterInfo parameterInfo)
            : base(string.Concat("method=", method, ",type=", parameterInfo.ParameterType)) { }
    }
}