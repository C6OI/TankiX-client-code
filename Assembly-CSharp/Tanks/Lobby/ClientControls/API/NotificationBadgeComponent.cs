using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientControls.API {
    public class NotificationBadgeComponent : BehaviourComponent {
        public bool BadgeActivity {
            set => gameObject.SetActive(value);
        }
    }
}