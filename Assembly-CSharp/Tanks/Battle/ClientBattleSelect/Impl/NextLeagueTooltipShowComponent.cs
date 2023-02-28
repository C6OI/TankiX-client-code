using Tanks.Lobby.ClientControls.API;
using UnityEngine;

namespace Tanks.Battle.ClientBattleSelect.Impl {
    public class NextLeagueTooltipShowComponent : TooltipShowBehaviour {
        [SerializeField] LocalizedField unfairMatchText;

        NextLeagueTooltipData leagueData;

        public bool IsMaxLeague { get; set; }

        public void SetNextLeagueTooltipData(double points, string icon, string name, int delta, bool unfairMM) {
            string unfairMM2 = !unfairMM ? string.Empty : unfairMatchText.Value;
            leagueData = new NextLeagueTooltipData(points, icon, name, delta, unfairMM2);
        }

        public override void ShowTooltip(Vector3 mousePosition) {
            CheckForTutorialEvent checkForTutorialEvent = new();
            EngineService.Engine.ScheduleEvent(checkForTutorialEvent, EngineService.EntityStub);

            if (!checkForTutorialEvent.TutorialIsActive) {
                tooltipShowed = true;

                if (IsMaxLeague) {
                    TooltipController.Instance.ShowTooltip(mousePosition, tipText);
                } else {
                    TooltipController.Instance.ShowTooltip(mousePosition, leagueData, customPrefab);
                }
            }
        }
    }
}