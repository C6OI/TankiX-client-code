using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class UpdateEventFireHandler : EventFireHandler {
        public UpdateEventFireHandler(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription)
            : base(typeof(UpdateEvent), method, methodHandle, handlerArgumentsDescription) { }
    }
}