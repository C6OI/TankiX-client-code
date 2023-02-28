using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public class TooltipWithHeaderShowComponent : TooltipShowBehaviour {
        public LocalizedField header;

        protected string headerTipText = string.Empty;

        public virtual string HeaderTipText {
            get => headerTipText;
            set => headerTipText = value;
        }

        protected override void Awake() {
            base.Awake();

            if (string.IsNullOrEmpty(headerTipText) && !string.IsNullOrEmpty(header.Value)) {
                HeaderTipText = header.Value;
            }
        }

        public override void ShowTooltip(Vector3 mousePosition) {
            CheckForTutorialEvent checkForTutorialEvent = new();
            EngineService.Engine.ScheduleEvent(checkForTutorialEvent, EngineService.EntityStub);

            if (!checkForTutorialEvent.TutorialIsActive) {
                tooltipShowed = true;
                string[] data = new string[2] { HeaderTipText, TipText };
                TooltipController.Instance.ShowTooltip(mousePosition, data, !customContentPrefab ? null : customPrefab);
            }
        }
    }
}