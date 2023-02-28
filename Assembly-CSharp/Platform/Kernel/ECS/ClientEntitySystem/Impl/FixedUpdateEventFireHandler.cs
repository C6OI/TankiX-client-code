using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class FixedUpdateEventFireHandler : EventFireHandler {
        public FixedUpdateEventFireHandler(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription)
            : base(typeof(FixedUpdateEvent), method, methodHandle, handlerArgumentsDescription) { }
    }
}