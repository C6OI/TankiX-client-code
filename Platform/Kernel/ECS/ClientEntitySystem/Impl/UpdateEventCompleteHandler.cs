using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class UpdateEventCompleteHandler : EventCompleteHandler {
        public UpdateEventCompleteHandler(MethodInfo method, MethodHandle methodHandle,
            HandlerArgumentsDescription handlerArgumentsDescription)
            : base(typeof(UpdateEvent), method, methodHandle, handlerArgumentsDescription) { }
    }
}