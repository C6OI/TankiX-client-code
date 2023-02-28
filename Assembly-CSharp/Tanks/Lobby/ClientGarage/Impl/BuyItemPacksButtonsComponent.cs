using System.Collections.Generic;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class BuyItemPacksButtonsComponent : BehaviourComponent {
        [SerializeField] List<EntityBehaviour> buyButtons;

        public List<EntityBehaviour> BuyButtons => buyButtons;

        public void SetBuyButtonsInactive() {
            buyButtons.ForEach(delegate(EntityBehaviour button) {
                button.gameObject.SetActive(false);
            });
        }
    }
}