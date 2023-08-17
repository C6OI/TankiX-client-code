using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.Hud.Impl {
    public class ItemButtonActivatedStateComponent : Component {
        public float Total { get; set; }

        public float Remaining { get; set; }
    }
}