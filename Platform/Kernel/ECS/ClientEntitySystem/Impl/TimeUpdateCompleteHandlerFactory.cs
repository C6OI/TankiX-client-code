using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class TimeUpdateCompleteHandlerFactory : BroadcastEventHandlerFactory {
        public TimeUpdateCompleteHandlerFactory()
            : base(typeof(OnEventComplete), typeof(TimeUpdateEvent)) { }

        protected override Handler CreateHandlerInstance(MethodInfo method, MethodHandle methodHandle,
            HandlerArgumentsDescription handlerArgumentsDescription) =>
            new TimeUpdateCompleteHandler(method, methodHandle, handlerArgumentsDescription);
    }
}