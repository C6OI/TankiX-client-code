using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class AfterFixedUpdateEventFireHandler : EventFireHandler {
        public AfterFixedUpdateEventFireHandler(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription)
            : base(typeof(AfterFixedUpdateEvent), method, methodHandle, handlerArgumentsDescription) { }
    }
}