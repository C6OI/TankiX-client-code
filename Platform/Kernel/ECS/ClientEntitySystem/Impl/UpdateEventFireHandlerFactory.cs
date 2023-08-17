using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class UpdateEventFireHandlerFactory : BroadcastEventHandlerFactory {
        public UpdateEventFireHandlerFactory()
            : base(typeof(OnEventFire), typeof(UpdateEvent)) { }

        protected override Handler CreateHandlerInstance(MethodInfo method, MethodHandle methodHandle,
            HandlerArgumentsDescription handlerArgumentsDescription) =>
            new UpdateEventFireHandler(method, methodHandle, handlerArgumentsDescription);
    }
}