using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class TutorialRewardsUIComponent : MonoBehaviour {
        [SerializeField] TextMeshProUGUI crysCount;

        [SerializeField] TextMeshProUGUI itemName;

        [SerializeField] ImageSkin item;

        [SerializeField] LocalizedField crysLocalizedField;

        [SerializeField] LocalizedField itemLocalizedField;

        public void SetupTutorialReward(long crys, string itemSpriteUID) {
            crysCount.text = crysLocalizedField.Value + " x" + crys;
            itemName.text = itemLocalizedField.Value;
            item.SpriteUid = itemSpriteUID;
        }
    }
}