namespace YamlDotNet.Serialization {
    public class ObjectEventInfo : EventInfo {
        protected ObjectEventInfo(IObjectDescriptor source)
            : base(source) { }

        public string Anchor { get; set; }

        public string Tag { get; set; }
    }
}