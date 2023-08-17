using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class TimeUpdateFireHandlerFactory : BroadcastEventHandlerFactory {
        public TimeUpdateFireHandlerFactory()
            : base(typeof(OnEventFire), typeof(TimeUpdateEvent)) { }

        protected override Handler CreateHandlerInstance(MethodInfo method, MethodHandle methodHandle,
            HandlerArgumentsDescription handlerArgumentsDescription) =>
            new TimeUpdateFireHandler(method, methodHandle, handlerArgumentsDescription);
    }
}