using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.API {
    public class HighlightTankStepHandler : TutorialStepHandler {
        [SerializeField] RectTransform popupPositionRect;

        [SerializeField] float cameraOffset;

        [SerializeField] GameObject outlinePrefab;

        [SerializeField] bool useOverlay;

        public override void RunStep(TutorialData data) {
            base.RunStep(data);
            data.Type = TutorialType.HighlightTankPart;
            data.PopupPositionRect = popupPositionRect;
            data.CameraOffset = cameraOffset;
            data.OutlinePrefab = outlinePrefab;
            data.ContinueOnClick = true;
            TutorialCanvas.Instance.Show(data, useOverlay);
        }
    }
}