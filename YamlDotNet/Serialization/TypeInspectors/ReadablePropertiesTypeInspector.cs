using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.TypeInspectors {
    public sealed class ReadablePropertiesTypeInspector : TypeInspectorSkeleton {
        readonly ITypeResolver _typeResolver;

        public ReadablePropertiesTypeInspector(ITypeResolver typeResolver) {
            if (typeResolver == null) {
                throw new ArgumentNullException("typeResolver");
            }

            _typeResolver = typeResolver;
        }

        static bool IsValidProperty(PropertyInfo property) =>
            property.CanRead && property.GetGetMethod().GetParameters().Length == 0;

        public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object container) => type
            .GetPublicProperties().Where(IsValidProperty)
            .Select((Func<PropertyInfo, IPropertyDescriptor>)(p => new ReflectionPropertyDescriptor(p, _typeResolver)));

        sealed class ReflectionPropertyDescriptor : IPropertyDescriptor {
            readonly PropertyInfo _propertyInfo;

            readonly ITypeResolver _typeResolver;

            public ReflectionPropertyDescriptor(PropertyInfo propertyInfo, ITypeResolver typeResolver) {
                _propertyInfo = propertyInfo;
                _typeResolver = typeResolver;
                ScalarStyle = ScalarStyle.Any;
            }

            public string Name => _propertyInfo.Name;

            public Type Type => _propertyInfo.PropertyType;

            public Type TypeOverride { get; set; }

            public int Order { get; set; }

            public bool CanWrite => _propertyInfo.CanWrite;

            public ScalarStyle ScalarStyle { get; set; }

            public void Write(object target, object value) => _propertyInfo.SetValue(target, value, null);

            public T GetCustomAttribute<T>() where T : Attribute {
                object[] customAttributes = _propertyInfo.GetCustomAttributes(typeof(T), true);
                return (T)customAttributes.FirstOrDefault();
            }

            public IObjectDescriptor Read(object target) {
                object obj = _propertyInfo.ReadValue(target);
                Type type = TypeOverride ?? _typeResolver.Resolve(Type, obj);
                return new ObjectDescriptor(obj, type, Type, ScalarStyle);
            }
        }
    }
}