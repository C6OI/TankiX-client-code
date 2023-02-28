using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientNotifications.API {
    public class UserRankRewardMoneyBlock : MonoBehaviour {
        [SerializeField] Text moneyRewardField;

        public Text MoneyRewardField => moneyRewardField;
    }
}