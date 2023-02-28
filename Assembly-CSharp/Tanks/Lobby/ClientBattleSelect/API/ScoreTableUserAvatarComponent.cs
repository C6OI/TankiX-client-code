using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class ScoreTableUserAvatarComponent : BehaviourComponent {
        [SerializeField] bool enableShowUserProfileOnAvatarClick;

        public bool EnableShowUserProfileOnAvatarClick => enableShowUserProfileOnAvatarClick;
    }
}