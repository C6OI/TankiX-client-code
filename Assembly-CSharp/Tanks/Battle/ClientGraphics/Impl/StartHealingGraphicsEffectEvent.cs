using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class StartHealingGraphicsEffectEvent : Event {
        public StartHealingGraphicsEffectEvent(float duration) => Duration = duration;

        public float Duration { get; set; }
    }
}