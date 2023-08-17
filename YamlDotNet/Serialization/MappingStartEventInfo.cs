using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization {
    public sealed class MappingStartEventInfo : ObjectEventInfo {
        public MappingStartEventInfo(IObjectDescriptor source)
            : base(source) { }

        public bool IsImplicit { get; set; }

        public MappingStyle Style { get; set; }
    }
}