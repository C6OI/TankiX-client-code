using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class TimeUpdateCompleteHandler : EventCompleteHandler {
        public TimeUpdateCompleteHandler(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription)
            : base(typeof(TimeUpdateEvent), method, methodHandle, handlerArgumentsDescription) { }
    }
}