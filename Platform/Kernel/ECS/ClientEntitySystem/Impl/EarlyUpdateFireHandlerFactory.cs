using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EarlyUpdateFireHandlerFactory : BroadcastEventHandlerFactory {
        public EarlyUpdateFireHandlerFactory()
            : base(typeof(OnEventFire), typeof(EarlyUpdateEvent)) { }

        protected override Handler CreateHandlerInstance(MethodInfo method, MethodHandle methodHandle,
            HandlerArgumentsDescription handlerArgumentsDescription) =>
            new EarlyUpdateFireHandler(method, methodHandle, handlerArgumentsDescription);
    }
}