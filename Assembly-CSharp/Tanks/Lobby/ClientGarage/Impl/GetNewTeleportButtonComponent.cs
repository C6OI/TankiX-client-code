using Platform.Library.ClientUnityIntegration.API;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GetNewTeleportButtonComponent : BehaviourComponent {
        void OnEnable() {
            GetComponent<Button>().interactable = true;
        }
    }
}