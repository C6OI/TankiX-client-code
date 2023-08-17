using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Lobby.ClientNavigation.API {
    public class DialogsComponent : BehaviourComponent {
        public T Get<T>() where T : MonoBehaviour => GetComponentInChildren<T>(true);

        public void CloseAll() {
            foreach (Transform item in transform) {
                item.gameObject.SetActive(false);
            }
        }
    }
}