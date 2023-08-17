using System;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization {
    public sealed class PropertyDescriptor : IPropertyDescriptor {
        readonly IPropertyDescriptor baseDescriptor;

        public PropertyDescriptor(IPropertyDescriptor baseDescriptor) {
            this.baseDescriptor = baseDescriptor;
            Name = baseDescriptor.Name;
        }

        public string Name { get; set; }

        public Type Type => baseDescriptor.Type;

        public Type TypeOverride {
            get => baseDescriptor.TypeOverride;
            set => baseDescriptor.TypeOverride = value;
        }

        public int Order { get; set; }

        public ScalarStyle ScalarStyle {
            get => baseDescriptor.ScalarStyle;
            set => baseDescriptor.ScalarStyle = value;
        }

        public bool CanWrite => baseDescriptor.CanWrite;

        public void Write(object target, object value) => baseDescriptor.Write(target, value);

        public T GetCustomAttribute<T>() where T : Attribute => baseDescriptor.GetCustomAttribute<T>();

        public IObjectDescriptor Read(object target) => baseDescriptor.Read(target);
    }
}