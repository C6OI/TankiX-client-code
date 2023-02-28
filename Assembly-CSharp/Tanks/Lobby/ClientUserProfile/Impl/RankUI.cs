using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class RankUI : MonoBehaviour {
        [SerializeField] ImageListSkin rankIcon;

        [SerializeField] TextMeshProUGUI rank;

        [SerializeField] LocalizedField rankLocalizedField;

        public void SetRank(int rankIconIndex, string rankName) {
            rank.text = rankName;
            rankIcon.SelectSprite((rankIconIndex + 1).ToString());
        }
    }
}