using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientUserProfile.API {
    [SerialVersionUID(635870034442836260L)]
    public class UserLabelComponent : BehaviourComponent {
        [SerializeField] long userId;

        public GameObject inBattleIcon;

        public bool SkipLoadUserFromServer { get; set; }

        public bool AllowInBattleIcon { get; set; }

        public long UserId {
            get => userId;
            set => userId = value;
        }
    }
}