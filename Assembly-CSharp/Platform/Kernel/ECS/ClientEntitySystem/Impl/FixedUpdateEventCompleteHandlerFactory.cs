using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class FixedUpdateEventCompleteHandlerFactory : BroadcastEventHandlerFactory {
        public FixedUpdateEventCompleteHandlerFactory()
            : base(typeof(OnEventComplete), typeof(FixedUpdateEvent)) { }

        protected override Handler CreateHandlerInstance(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) =>
            new FixedUpdateEventCompleteHandler(method, methodHandle, handlerArgumentsDescription);
    }
}