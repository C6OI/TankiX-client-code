using Platform.Kernel.ECS.ClientEntitySystem.Impl;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public class SingleNode<T> : AbstractSingleNode where T : Component {
        public T component;
    }
}