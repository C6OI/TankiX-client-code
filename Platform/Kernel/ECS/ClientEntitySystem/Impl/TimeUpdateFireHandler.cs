using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class TimeUpdateFireHandler : EventFireHandler {
        public TimeUpdateFireHandler(MethodInfo method, MethodHandle methodHandle,
            HandlerArgumentsDescription handlerArgumentsDescription)
            : base(typeof(TimeUpdateEvent), method, methodHandle, handlerArgumentsDescription) { }
    }
}