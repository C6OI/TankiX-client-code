using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class SelfDestructionConfigComponent : Component {
        public int SuicideDurationTime { get; set; }
    }
}