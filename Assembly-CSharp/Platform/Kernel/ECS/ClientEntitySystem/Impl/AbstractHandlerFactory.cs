using System;
using System.Collections.Generic;
using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public abstract class AbstractHandlerFactory : HandlerFactory {
        readonly Type annotationEventClass;

        readonly IList<Type> parameterClasses;

        protected internal AbstractHandlerFactory(Type annotationEventClass, IList<Type> parameterClasses) {
            this.annotationEventClass = annotationEventClass;
            this.parameterClasses = parameterClasses;
        }

        public Handler CreateHandler(MethodInfo method, ECSSystem system) {
            if (!IsSelf(method)) {
                return null;
            }

            ValidateMethodIsPublic(method);
            ValidateEventTypes(method);
            HandlerArgumentsDescription handlerArgumentsDescription = new HandlerArgumentsParser(method).Parse();
            Validate(method, handlerArgumentsDescription);
            return CreateHandlerInstance(method, GetMethodHandle(method, system), handlerArgumentsDescription);
        }

        void ValidateMethodIsPublic(MethodInfo method) {
            if (!method.IsPublic) {
                throw new HandlerNotPublicException(method);
            }
        }

        public MethodHandle GetMethodHandle(MethodInfo method, ECSSystem system) => new(method, system);

        protected virtual bool IsSelf(MethodInfo method) {
            object[] customAttributes = method.GetCustomAttributes(annotationEventClass, true);
            return customAttributes.Length == 1;
        }

        void ValidateEventTypes(MethodInfo method) {
            ParameterInfo[] parameters = method.GetParameters();

            if (parameters.Length < parameterClasses.Count) {
                throw new EventAbsentInArgumentHandlerFactoryException(method);
            }

            for (int i = 0; i < parameterClasses.Count; i++) {
                if (!parameterClasses[i].IsAssignableFrom(parameters[i].ParameterType)) {
                    throw new EventAbsentInArgumentHandlerFactoryException(method, parameterClasses[i]);
                }
            }
        }

        protected virtual void Validate(MethodInfo method, HandlerArgumentsDescription argumentsDescription) { }

        protected abstract Handler CreateHandlerInstance(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription);
    }
}