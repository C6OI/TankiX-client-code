using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientProfile.API {
    [Shared]
    [SerialVersionUID(1473074767785L)]
    public class UserXCrystalsComponent : Component, ComponentServerChangeListener {
        [Inject] public static EngineService EngineService { get; set; }

        public long Money { get; set; }

        public void ChangedOnServer(Entity entity) {
            EngineService.Engine.ScheduleEvent<UserXCrystalsChangedEvent>(entity);
        }
    }
}