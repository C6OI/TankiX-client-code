using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientQuests.Impl {
    public class PickUpQuestRewardButtonComponent : BehaviourComponent {
        public Entity QuestEntity { get; set; }
    }
}