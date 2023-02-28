using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class CancelNotificationComponent : BehaviourComponent {
        Entity entity;

        void Awake() {
            enabled = false;
        }

        void Update() {
            if (InputMapping.Cancel) {
                EngineService.Engine.ScheduleEvent<CloseNotificationEvent>(entity);
            }
        }

        public void Init(Entity entity) {
            this.entity = entity;
            enabled = true;
        }
    }
}