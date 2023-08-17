using System;
using System.Reflection;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public abstract class ConcreteEventHandlerFactory : AbstractHandlerFactory {
        readonly Type parameterClass;

        protected ConcreteEventHandlerFactory(Type annotationEventClass, Type parameterClass)
            : base(annotationEventClass, Collections.SingletonList(parameterClass)) => this.parameterClass = parameterClass;

        protected override bool IsSelf(MethodInfo method) {
            if (base.IsSelf(method)) {
                ParameterInfo[] parameters = method.GetParameters();
                return parameters.Length > 0 && parameters[0].ParameterType == parameterClass;
            }

            return false;
        }
    }
}