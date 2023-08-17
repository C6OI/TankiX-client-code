using System;
using System.Collections.Generic;
using System.Linq;

namespace YamlDotNet.Serialization.TypeInspectors {
    public sealed class NamingConventionTypeInspector : TypeInspectorSkeleton {
        readonly ITypeInspector innerTypeDescriptor;

        readonly INamingConvention namingConvention;

        public NamingConventionTypeInspector(ITypeInspector innerTypeDescriptor, INamingConvention namingConvention) {
            if (innerTypeDescriptor == null) {
                throw new ArgumentNullException("innerTypeDescriptor");
            }

            this.innerTypeDescriptor = innerTypeDescriptor;

            if (namingConvention == null) {
                throw new ArgumentNullException("namingConvention");
            }

            this.namingConvention = namingConvention;
        }

        public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object container) => innerTypeDescriptor
            .GetProperties(type, container).Select(
                (Func<IPropertyDescriptor, IPropertyDescriptor>)(p => new PropertyDescriptor(p) {
                                                                        Name = namingConvention.Apply(p.Name)
                                                                    }));
    }
}