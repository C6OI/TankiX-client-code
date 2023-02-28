using System;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class SpecialOfferOpenTutorialContainerButton : BehaviourComponent {
        public long containerId;

        public int quantity;

        public Action onOpen;
    }
}