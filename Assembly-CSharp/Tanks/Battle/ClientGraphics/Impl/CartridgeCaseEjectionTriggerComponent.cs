namespace Tanks.Battle.ClientGraphics.Impl {
    public class CartridgeCaseEjectionTriggerComponent : AnimationTriggerComponent {
        public void OnCaseEject() {
            ProvideEvent<CartridgeCaseEjectionEvent>();
        }
    }
}