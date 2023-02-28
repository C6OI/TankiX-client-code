using System;
using System.Reflection;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EventCompleteHandler : Handler {
        public EventCompleteHandler(Type eventType, MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription)
            : base(EventPhase.Complete, eventType, method, methodHandle, handlerArgumentsDescription) { }
    }
}