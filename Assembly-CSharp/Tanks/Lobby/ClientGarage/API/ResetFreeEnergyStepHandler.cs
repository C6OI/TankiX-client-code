using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    public class ResetFreeEnergyStepHandler : AddItemStepHandler {
        public override void RunStep(TutorialData data) {
            ShopTabManager.shopTabIndex = 0;
            base.RunStep(data);
        }
    }
}