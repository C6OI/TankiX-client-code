using System;
using System.Reflection;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class ComponentInfoNotFoundException : Exception {
        public ComponentInfoNotFoundException(Type infoType, MethodInfo componentMethod)
            : base(string.Concat("infoType=", infoType, " componentMethod=", componentMethod)) { }
    }
}