using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class UserInfoUIComponent : BehaviourComponent {
        [SerializeField] GameObject defaultInfo;

        [SerializeField] GameObject squadInfo;

        public void SwitchSquadInfo(bool value) {
            squadInfo.SetActive(value);
            defaultInfo.SetActive(!value);
        }
    }
}