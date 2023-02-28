using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientUserProfile.API {
    [SerialVersionUID(1507285157772L)]
    public class UserLabelWaitIntiveResponseIconComponent : BehaviourComponent {
        [SerializeField] public GameObject icon;

        public bool Wait {
            set => icon.SetActive(value);
        }
    }
}