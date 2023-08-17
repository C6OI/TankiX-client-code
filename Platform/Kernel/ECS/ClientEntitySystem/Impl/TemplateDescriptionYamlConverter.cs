using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class TemplateDescriptionYamlConverter : IYamlTypeConverter {
        readonly TemplateRegistry templateRegistry;

        public TemplateDescriptionYamlConverter(TemplateRegistry templateRegistry) =>
            this.templateRegistry = templateRegistry;

        public bool Accepts(Type type) => type == typeof(TemplateDescription);

        public object ReadYaml(IParser parser, Type type) {
            long id = long.Parse(((Scalar)parser.Current).Value);
            parser.MoveNext();
            return templateRegistry.GetTemplateInfo(id);
        }

        public void WriteYaml(IEmitter emitter, object value, Type type) => throw new NotImplementedException();
    }
}