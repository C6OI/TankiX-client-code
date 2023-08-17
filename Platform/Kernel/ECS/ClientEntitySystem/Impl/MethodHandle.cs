using System;
using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class MethodHandle {
        readonly MethodInfo method;

        readonly ECSSystem system;

        readonly bool throwInnerException;

        public MethodHandle(MethodInfo method, ECSSystem system) {
            this.method = method;
            this.system = system;
            throwInnerException = TestContext.IsTestMode;
        }

        public object Invoke(object[] args) {
            if (throwInnerException) {
                try {
                    return method.Invoke(system, args);
                } catch (TargetInvocationException ex) {
                    MethodInfo methodInfo = typeof(Exception).GetMethod("InternalPreserveStackTrace",
                        BindingFlags.Instance | BindingFlags.NonPublic);

                    if (methodInfo != null) {
                        methodInfo.Invoke(ex.InnerException, null);
                    }

                    throw ex.InnerException;
                }
            }

            return method.Invoke(system, args);
        }
    }
}