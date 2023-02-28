using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Battle.ClientBattleSelect.Impl {
    public class NextLeagueTooltipContent : MonoBehaviour, ITooltipContent {
        [SerializeField] TextMeshProUGUI text;

        [SerializeField] ImageSkin leagueIcon;

        [SerializeField] TextMeshProUGUI leagueName;

        [SerializeField] LocalizedField leaguePointsText;

        [SerializeField] LocalizedField leagueNameText;

        public void Init(object data) {
            NextLeagueTooltipData nextLeagueTooltipData = data as NextLeagueTooltipData;

            if (!string.IsNullOrEmpty(nextLeagueTooltipData.unfairMM)) {
                text.text = nextLeagueTooltipData.unfairMM + "\n";
            }

            text.text += string.Format(leaguePointsText, nextLeagueTooltipData.points);
            leagueIcon.SpriteUid = nextLeagueTooltipData.icon;
            leagueName.text = string.Format(leagueNameText, nextLeagueTooltipData.name);
        }
    }
}