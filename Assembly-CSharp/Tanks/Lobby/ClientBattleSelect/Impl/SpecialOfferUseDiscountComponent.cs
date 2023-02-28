using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class SpecialOfferUseDiscountComponent : BehaviourComponent {
        public void OnClick() {
            ScheduleEvent(new ShowXCrystalsDialogEvent {
                ShowTitle = true
            }, GetComponent<EntityBehaviour>().Entity);
        }
    }
}