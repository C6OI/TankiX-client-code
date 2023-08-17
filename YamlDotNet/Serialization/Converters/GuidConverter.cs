using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.Converters {
    public class GuidConverter : IYamlTypeConverter {
        public bool Accepts(Type type) => type == typeof(Guid);

        public object ReadYaml(IParser parser, Type type) {
            string value = ((Scalar)parser.Current).Value;
            parser.MoveNext();
            return new Guid(value);
        }

        public void WriteYaml(IEmitter emitter, object value, Type type) =>
            emitter.Emit(new Scalar(((Guid)value).ToString("D")));
    }
}