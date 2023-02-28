using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NodeAddedFireHandlerFactory : ConcreteEventHandlerFactory {
        public NodeAddedFireHandlerFactory()
            : base(typeof(OnEventFire), typeof(NodeAddedEvent)) { }

        protected override Handler CreateHandlerInstance(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) =>
            new NodeAddedFireHandler(method, methodHandle, handlerArgumentsDescription);
    }
}