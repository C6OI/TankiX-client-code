using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EntityYamlConverter : IYamlTypeConverter {
        readonly EngineServiceInternal engine;

        public EntityYamlConverter(EngineServiceInternal engine) => this.engine = engine;

        public bool Accepts(Type type) => typeof(Entity).IsAssignableFrom(type);

        public object ReadYaml(IParser parser, Type type) {
            string value = ((Scalar)parser.Current).Value;
            parser.MoveNext();
            return engine.EntityRegistry.GetEntity(ConfigurationEntityIdCalculator.Calculate(value));
        }

        public void WriteYaml(IEmitter emitter, object value, Type type) {
            throw new NotImplementedException();
        }
    }
}