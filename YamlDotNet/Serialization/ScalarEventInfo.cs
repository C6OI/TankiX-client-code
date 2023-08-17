using YamlDotNet.Core;

namespace YamlDotNet.Serialization {
    public sealed class ScalarEventInfo : ObjectEventInfo {
        public ScalarEventInfo(IObjectDescriptor source)
            : base(source) => Style = source.ScalarStyle;

        public string RenderedValue { get; set; }

        public ScalarStyle Style { get; set; }

        public bool IsPlainImplicit { get; set; }

        public bool IsQuotedImplicit { get; set; }
    }
}