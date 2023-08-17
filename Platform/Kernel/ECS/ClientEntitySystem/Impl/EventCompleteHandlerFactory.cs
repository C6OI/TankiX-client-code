using System;
using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EventCompleteHandlerFactory : AbstractHandlerFactory {
        public EventCompleteHandlerFactory()
            : base(typeof(OnEventComplete), Collections.SingletonList(typeof(Event))) { }

        protected override Handler CreateHandlerInstance(MethodInfo method, MethodHandle methodHandle,
            HandlerArgumentsDescription handlerArgumentsDescription) {
            Type parameterType = method.GetParameters()[0].ParameterType;
            return new EventCompleteHandler(parameterType, method, methodHandle, handlerArgumentsDescription);
        }
    }
}