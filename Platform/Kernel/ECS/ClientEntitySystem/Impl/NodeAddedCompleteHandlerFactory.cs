using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NodeAddedCompleteHandlerFactory : ConcreteEventHandlerFactory {
        public NodeAddedCompleteHandlerFactory()
            : base(typeof(OnEventComplete), typeof(NodeAddedEvent)) { }

        protected override Handler CreateHandlerInstance(MethodInfo method, MethodHandle methodHandle,
            HandlerArgumentsDescription handlerArgumentsDescription) =>
            new NodeAddedCompleteHandler(method, methodHandle, handlerArgumentsDescription);
    }
}