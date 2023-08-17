using System;
using System.Collections.Generic;

namespace YamlDotNet.Serialization.ObjectFactories {
    public sealed class DefaultObjectFactory : IObjectFactory {
        static readonly Dictionary<Type, Type> defaultInterfaceImplementations = new() {
            {
                typeof(IEnumerable<>),
                typeof(List<>)
            }, {
                typeof(ICollection<>),
                typeof(List<>)
            }, {
                typeof(IList<>),
                typeof(List<>)
            }, {
                typeof(IDictionary<,>),
                typeof(Dictionary<,>)
            }
        };

        public object Create(Type type) {
            Type value;

            if (type.IsInterface() &&
                defaultInterfaceImplementations.TryGetValue(type.GetGenericTypeDefinition(), out value)) {
                type = value.MakeGenericType(type.GetGenericArguments());
            }

            try {
                return Activator.CreateInstance(type);
            } catch (Exception innerException) {
                string message = string.Format("Failed to create an instance of type '{0}'.", type);
                throw new InvalidOperationException(message, innerException);
            }
        }
    }
}