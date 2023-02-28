using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NodeRemovedFireHandler : EventFireHandler {
        public NodeRemovedFireHandler(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription)
            : base(typeof(NodeRemoveEvent), method, methodHandle, handlerArgumentsDescription) { }
    }
}