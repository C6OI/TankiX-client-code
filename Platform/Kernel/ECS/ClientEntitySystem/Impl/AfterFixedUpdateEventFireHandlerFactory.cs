using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class AfterFixedUpdateEventFireHandlerFactory : BroadcastEventHandlerFactory {
        public AfterFixedUpdateEventFireHandlerFactory()
            : base(typeof(OnEventFire), typeof(AfterFixedUpdateEvent)) { }

        protected override Handler CreateHandlerInstance(MethodInfo method, MethodHandle methodHandle,
            HandlerArgumentsDescription handlerArgumentsDescription) =>
            new AfterFixedUpdateEventFireHandler(method, methodHandle, handlerArgumentsDescription);
    }
}