using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class TutorialHideTriggerComponent : BehaviourComponent {
        [SerializeField] float hideDelay;

        protected bool triggered;

        protected Entity tutorialStep;

        void OnDisable() {
            ForceHide();
        }

        public void Activate(Entity tutorialStep) {
            this.tutorialStep = tutorialStep;
            gameObject.SetActive(true);

            if (!gameObject.activeInHierarchy) {
                ForceHide();
            }
        }

        protected virtual void Triggered() {
            if (!triggered) {
                CancelInvoke();
                triggered = true;

                if (hideDelay == 0f) {
                    HideTutorial();
                } else {
                    Invoke("HideTutorial", hideDelay);
                }
            }
        }

        void HideTutorial() {
            TutorialCanvas.Instance.Hide();
            gameObject.SetActive(false);
        }

        public void ForceHide() {
            if (!triggered) {
                ScheduleEvent<CompleteActiveTutorialEvent>(new EntityStub());
                hideDelay = 0f;
                Triggered();
            }
        }
    }
}