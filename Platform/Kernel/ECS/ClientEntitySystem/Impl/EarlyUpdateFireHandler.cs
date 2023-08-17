using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EarlyUpdateFireHandler : EventFireHandler {
        public EarlyUpdateFireHandler(MethodInfo method, MethodHandle methodHandle,
            HandlerArgumentsDescription handlerArgumentsDescription)
            : base(typeof(EarlyUpdateEvent), method, methodHandle, handlerArgumentsDescription) { }
    }
}