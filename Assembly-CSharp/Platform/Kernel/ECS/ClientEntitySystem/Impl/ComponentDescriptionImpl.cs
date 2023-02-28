using System;
using System.Collections.Generic;
using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class ComponentDescriptionImpl : ComponentDescription {
        readonly MethodInfo componentMethod;

        readonly IDictionary<Type, ComponentInfo> infos;

        public ComponentDescriptionImpl(MethodInfo componentMethod) {
            infos = new Dictionary<Type, ComponentInfo>();
            FieldName = componentMethod.Name;
            this.componentMethod = componentMethod;
            ComponentType = _getComponentType(componentMethod);
        }

        public string FieldName { get; }

        public Type ComponentType { get; }

        public T GetInfo<T>() where T : ComponentInfo {
            Type typeFromHandle = typeof(T);

            if (!infos.ContainsKey(typeFromHandle)) {
                throw new ComponentInfoNotFoundException(typeFromHandle, componentMethod);
            }

            return (T)infos[typeFromHandle];
        }

        public bool IsInfoPresent(Type infoType) => infos.ContainsKey(infoType);

        public void CollectInfo(ICollection<ComponentInfoBuilder> builders) {
            foreach (ComponentInfoBuilder builder in builders) {
                if (builder.IsAcceptable(componentMethod)) {
                    infos[builder.TemplateComponentInfoClass] = builder.Build(componentMethod, this);
                }
            }
        }

        Type _getComponentType(MethodInfo componentMethod) {
            Type returnType = componentMethod.ReturnType;

            if (!typeof(Component).IsAssignableFrom(returnType)) {
                throw new WrongComponentTypeException(returnType);
            }

            return returnType;
        }
    }
}