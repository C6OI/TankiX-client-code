namespace YamlDotNet.Serialization {
    public abstract class EventInfo {
        protected EventInfo(IObjectDescriptor source) => Source = source;

        public IObjectDescriptor Source { get; private set; }
    }
}