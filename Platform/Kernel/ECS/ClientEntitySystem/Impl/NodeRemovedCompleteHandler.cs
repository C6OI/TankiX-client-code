using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NodeRemovedCompleteHandler : EventCompleteHandler {
        public NodeRemovedCompleteHandler(MethodInfo method, MethodHandle methodHandle,
            HandlerArgumentsDescription handlerArgumentsDescription)
            : base(typeof(NodeRemoveEvent), method, methodHandle, handlerArgumentsDescription) { }
    }
}