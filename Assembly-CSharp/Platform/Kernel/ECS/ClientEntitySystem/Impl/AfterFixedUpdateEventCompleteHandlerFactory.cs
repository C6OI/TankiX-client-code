using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class AfterFixedUpdateEventCompleteHandlerFactory : BroadcastEventHandlerFactory {
        public AfterFixedUpdateEventCompleteHandlerFactory()
            : base(typeof(OnEventComplete), typeof(AfterFixedUpdateEvent)) { }

        protected override Handler CreateHandlerInstance(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) =>
            new AfterFixedUpdateEventCompleteHandler(method, methodHandle, handlerArgumentsDescription);
    }
}