using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientQuests.Impl {
    public class QuestProgressGUIComponent : BehaviourComponent {
        [SerializeField] AnimatedProgress progress;

        [SerializeField] TextMeshProUGUI currentProgressValue;

        [SerializeField] TextMeshProUGUI targetProgressValue;

        [SerializeField] TextMeshProUGUI deltaProgressValue;

        [SerializeField] Animator deltaProgressAnimator;

        public string TargetProgressValue {
            get => targetProgressValue.text;
            set => targetProgressValue.text = value;
        }

        public string CurrentProgressValue {
            get => currentProgressValue.text;
            set => currentProgressValue.text = value;
        }

        public string DeltaProgressValue {
            get => deltaProgressValue.text;
            set {
                deltaProgressValue.text = value;
                deltaProgressAnimator.SetTrigger("ShowProgressDelta");
            }
        }

        public void Initialize(float currentValue, float targetValue) {
            progress.InitialNormalizedValue = currentValue / targetValue;
            CurrentProgressValue = currentValue.ToString();
        }

        public void Set(float currentValue, float targetValue) {
            progress.NormalizedValue = currentValue / targetValue;
            CurrentProgressValue = currentValue.ToString();
        }

        void DisableOutsideClickingOption() { }

        void EnableOutsideClickingOption() { }
    }
}