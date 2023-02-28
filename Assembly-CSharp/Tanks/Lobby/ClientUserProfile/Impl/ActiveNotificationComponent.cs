using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class ActiveNotificationComponent : BehaviourComponent {
        [SerializeField] Animator animator;

        [SerializeField] string showState = "Show";

        [SerializeField] string hideState = "Hide";

        [SerializeField] Text text;

        public ActiveNotificationComponent() {
            ShowState = Animator.StringToHash(showState);
            HideState = Animator.StringToHash(hideState);
        }

        public Entity Entity { get; set; }

        public Animator Animator => animator;

        public int ShowState { get; }

        public int HideState { get; }

        public Text Text => text;

        public bool Visible { get; private set; }

        public void Show() {
            Visible = true;

            if (Animator != null) {
                Animator.Play(ShowState);
            }
        }

        public void Hide() {
            Visible = false;

            if (Animator != null) {
                Animator.Play(HideState);

                if (Animator.parameters.Any(p => p.name.Equals("HideFlag"))) {
                    Animator.SetBool("HideFlag", true);
                }
            }
        }

        public void OnHidden() {
            EngineService.Engine.ScheduleEvent<NotificationShownEvent>(Entity);
        }
    }
}