using System;
using System.Collections.Generic;

namespace YamlDotNet.Serialization.TypeInspectors {
    public sealed class CachedTypeInspector : TypeInspectorSkeleton {
        readonly Dictionary<Type, List<IPropertyDescriptor>> cache = new();
        readonly ITypeInspector innerTypeDescriptor;

        public CachedTypeInspector(ITypeInspector innerTypeDescriptor) {
            if (innerTypeDescriptor == null) {
                throw new ArgumentNullException("innerTypeDescriptor");
            }

            this.innerTypeDescriptor = innerTypeDescriptor;
        }

        public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object container) {
            List<IPropertyDescriptor> value;

            if (!cache.TryGetValue(type, out value)) {
                return new List<IPropertyDescriptor>(innerTypeDescriptor.GetProperties(type, container));
            }

            return value;
        }
    }
}