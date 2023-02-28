using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientQuests.Impl {
    public class InBattleQuestItemGUIRewardComponent : MonoBehaviour {
        [SerializeField] TankPartItemIcon itemIcon;

        [SerializeField] TextMeshProUGUI quantityText;

        public void SetReward(int quantity) {
            quantityText.text = quantity.ToString();
        }

        public void SetReward(int quantity, long itemId) {
            SetReward(quantity);
            itemIcon.SetIconWithName(itemId.ToString());
        }
    }
}