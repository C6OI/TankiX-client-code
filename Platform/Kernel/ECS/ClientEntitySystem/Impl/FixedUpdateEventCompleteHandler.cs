using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class FixedUpdateEventCompleteHandler : EventCompleteHandler {
        public FixedUpdateEventCompleteHandler(MethodInfo method, MethodHandle methodHandle,
            HandlerArgumentsDescription handlerArgumentsDescription)
            : base(typeof(FixedUpdateEvent), method, methodHandle, handlerArgumentsDescription) { }
    }
}