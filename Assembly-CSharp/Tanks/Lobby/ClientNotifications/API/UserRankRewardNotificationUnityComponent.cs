using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientNotifications.API {
    public class UserRankRewardNotificationUnityComponent : BehaviourComponent {
        [SerializeField] Text rankHeaderElement;

        [SerializeField] Text rankNameElement;

        [SerializeField] ImageListSkin rankImageSkin;

        [SerializeField] UserRankRewardMoneyBlock xCrystalsBlock;

        [SerializeField] UserRankRewardMoneyBlock crystalsBlock;

        public ImageListSkin RankImageSkin => rankImageSkin;

        public Text RankHeaderElement => rankHeaderElement;

        public Text RankNameElement => rankNameElement;

        public UserRankRewardMoneyBlock XCrystalsBlock => xCrystalsBlock;

        public UserRankRewardMoneyBlock CrystalsBlock => crystalsBlock;
    }
}