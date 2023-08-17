namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftShotAnimationTriggerComponent : AnimationTriggerComponent {
        void OnCooldownStart() => ProvideEvent<ShaftShotAnimationCooldownStartEvent>();

        void OnCooldownClosing() => ProvideEvent<ShaftShotAnimationCooldownClosingEvent>();

        void OnCooldownEnd() => ProvideEvent<ShaftShotAnimationCooldownEndEvent>();
    }
}