namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NodeFieldMustBeComponentTypeException : ECSNotRunningException {
        public NodeFieldMustBeComponentTypeException(string str)
            : base(str) { }
    }
}