using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class UserEmailShowButtonComponent : BehaviourComponent {
        [SerializeField] Image icon;

        [SerializeField] Color visibleColor;

        [SerializeField] Color invisibleColor;

        public void SetEyeColor(bool visibly) {
            icon.color = !visibly ? visibleColor : invisibleColor;
        }
    }
}