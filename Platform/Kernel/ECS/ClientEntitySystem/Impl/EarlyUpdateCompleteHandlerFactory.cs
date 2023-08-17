using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EarlyUpdateCompleteHandlerFactory : BroadcastEventHandlerFactory {
        public EarlyUpdateCompleteHandlerFactory()
            : base(typeof(OnEventComplete), typeof(EarlyUpdateEvent)) { }

        protected override Handler CreateHandlerInstance(MethodInfo method, MethodHandle methodHandle,
            HandlerArgumentsDescription handlerArgumentsDescription) =>
            new EarlyUpdateCompleteHandler(method, methodHandle, handlerArgumentsDescription);
    }
}