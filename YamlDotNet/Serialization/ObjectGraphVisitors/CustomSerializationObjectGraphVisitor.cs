using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.ObjectGraphVisitors {
    public sealed class CustomSerializationObjectGraphVisitor : ChainedObjectGraphVisitor {
        readonly IEmitter emitter;

        readonly IEnumerable<IYamlTypeConverter> typeConverters;

        public CustomSerializationObjectGraphVisitor(IEmitter emitter, IObjectGraphVisitor nextVisitor,
            IEnumerable<IYamlTypeConverter> typeConverters)
            : base(nextVisitor) {
            this.emitter = emitter;
            IEnumerable<IYamlTypeConverter> enumerable2;

            if (typeConverters != null) {
                IEnumerable<IYamlTypeConverter> enumerable = typeConverters.ToList();
                enumerable2 = enumerable;
            } else {
                enumerable2 = Enumerable.Empty<IYamlTypeConverter>();
            }

            this.typeConverters = enumerable2;
        }

        public override bool Enter(IObjectDescriptor value) {
            IYamlTypeConverter yamlTypeConverter = typeConverters.FirstOrDefault(t => t.Accepts(value.Type));

            if (yamlTypeConverter != null) {
                yamlTypeConverter.WriteYaml(emitter, value.Value, value.Type);
                return false;
            }

            IYamlSerializable yamlSerializable = value as IYamlSerializable;

            if (yamlSerializable != null) {
                yamlSerializable.WriteYaml(emitter);
                return false;
            }

            return base.Enter(value);
        }
    }
}