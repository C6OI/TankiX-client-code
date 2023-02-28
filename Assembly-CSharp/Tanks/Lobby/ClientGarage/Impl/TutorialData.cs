using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class TutorialData {
        public TutorialType Type { get; set; }

        public Entity TutorialStep { get; set; }

        public Button InteractableButton { get; set; }

        public RectTransform PopupPositionRect { get; set; }

        public float ShowDelay { get; set; }

        public string Message { get; set; }

        public string ImageUid { get; set; }

        public float CameraOffset { get; set; }

        public bool HighlightHull { get; set; }

        public bool HighlightWeapon { get; set; }

        public GameObject OutlinePrefab { get; set; }

        public bool ContinueOnClick { get; set; }

        public int CurrentStepNumber { get; set; }

        public int StepCountInTutorial { get; set; }

        public bool InBattleMode { get; set; }
    }
}