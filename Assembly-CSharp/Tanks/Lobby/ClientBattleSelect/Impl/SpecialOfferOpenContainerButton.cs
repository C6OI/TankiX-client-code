using System;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class SpecialOfferOpenContainerButton : BehaviourComponent {
        public long containerId;

        public int quantity;

        public Action onOpen;
    }
}