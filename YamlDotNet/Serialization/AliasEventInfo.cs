namespace YamlDotNet.Serialization {
    public class AliasEventInfo : EventInfo {
        public AliasEventInfo(IObjectDescriptor source)
            : base(source) { }

        public string Alias { get; set; }
    }
}