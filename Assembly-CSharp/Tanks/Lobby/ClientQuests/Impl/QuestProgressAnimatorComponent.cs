using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientQuests.Impl {
    public class QuestProgressAnimatorComponent : Component {
        public float ProgressPrevValue { get; set; }
    }
}