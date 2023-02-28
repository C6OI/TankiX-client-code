using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NodeAddedCompleteHandler : EventCompleteHandler {
        public NodeAddedCompleteHandler(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription)
            : base(typeof(NodeAddedEvent), method, methodHandle, handlerArgumentsDescription) { }
    }
}