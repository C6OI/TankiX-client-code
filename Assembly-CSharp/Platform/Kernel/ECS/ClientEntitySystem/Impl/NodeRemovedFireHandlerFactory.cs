using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NodeRemovedFireHandlerFactory : ConcreteEventHandlerFactory {
        public NodeRemovedFireHandlerFactory()
            : base(typeof(OnEventFire), typeof(NodeRemoveEvent)) { }

        protected override Handler CreateHandlerInstance(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) =>
            new NodeRemovedFireHandler(method, methodHandle, handlerArgumentsDescription);
    }
}