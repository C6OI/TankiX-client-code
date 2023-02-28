using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    public class RankRestrictionGUIComponent : BehaviourComponent {
        [SerializeField] TextMeshProUGUI rankName;

        [SerializeField] ImageListSkin imageListSkin;

        public string RankName {
            set => rankName.text = value;
        }

        public void SetRank(int rank) {
            imageListSkin.SelectSprite(rank.ToString());
        }
    }
}