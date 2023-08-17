using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class IndexComponent : Component {
        public IndexComponent() { }

        public IndexComponent(int index) => Index = index;

        public int Index { get; set; }
    }
}