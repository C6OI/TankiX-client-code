namespace Tanks.Battle.ClientGraphics.Impl {
    public class CaseEjectorMovementTriggerComponent : AnimationTriggerComponent {
        void OnCaseEjectorOpen() => ProvideEvent<CaseEjectorOpenEvent>();

        void OnCaseEjectorClose() => ProvideEvent<CaseEjectorCloseEvent>();
    }
}