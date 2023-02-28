using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NodeAddedFireHandler : EventFireHandler {
        public NodeAddedFireHandler(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription)
            : base(typeof(NodeAddedEvent), method, methodHandle, handlerArgumentsDescription) { }
    }
}