using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class FixedUpdateEventFireHandlerFactory : BroadcastEventHandlerFactory {
        public FixedUpdateEventFireHandlerFactory()
            : base(typeof(OnEventFire), typeof(FixedUpdateEvent)) { }

        protected override Handler CreateHandlerInstance(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) =>
            new FixedUpdateEventFireHandler(method, methodHandle, handlerArgumentsDescription);
    }
}