using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization {
    public sealed class SequenceStartEventInfo : ObjectEventInfo {
        public SequenceStartEventInfo(IObjectDescriptor source)
            : base(source) { }

        public bool IsImplicit { get; set; }

        public SequenceStyle Style { get; set; }
    }
}