using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class UpdateEventCompleteHandlerFactory : BroadcastEventHandlerFactory {
        public UpdateEventCompleteHandlerFactory()
            : base(typeof(OnEventComplete), typeof(UpdateEvent)) { }

        protected override Handler CreateHandlerInstance(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) =>
            new UpdateEventCompleteHandler(method, methodHandle, handlerArgumentsDescription);
    }
}