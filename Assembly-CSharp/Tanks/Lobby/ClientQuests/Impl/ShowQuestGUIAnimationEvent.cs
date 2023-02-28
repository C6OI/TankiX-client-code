using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientQuests.Impl {
    public class ShowQuestGUIAnimationEvent : Event {
        public float ProgressDelay { get; set; }
    }
}