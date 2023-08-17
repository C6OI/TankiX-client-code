using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientUserProfile.Impl {
    public class ActiveNotificationComponent : BehaviourComponent {
        [SerializeField] Animator animator;

        [SerializeField] string hideState = "Hide";

        [SerializeField] Text text;

        public ActiveNotificationComponent() => HideState = Animator.StringToHash(hideState);

        public Animator Animator => animator;

        public int HideState { get; }

        public Text Text => text;

        public void Hide() => Animator.Play(HideState);

        public void OnHidden() {
            Entity entity = gameObject.GetComponent<EntityBehaviour>().Entity;

            ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine e) {
                e.ScheduleEvent<NotificationShownEvent>(entity);
            });
        }
    }
}