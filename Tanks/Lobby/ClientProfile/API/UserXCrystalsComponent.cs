using System.Runtime.CompilerServices;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientProfile.API {
    [Shared]
    [SerialVersionUID(1473074767785L)]
    public class UserXCrystalsComponent : Component, ComponentLifecycle {
        [Inject] public static EngineService EngineService { get; set; }

        public long Money { get; set; }

        void ComponentLifecycle.AttachToEntity(Entity entity) {
            if (entity != null) {
                EngineService.ExecuteInFlow(delegate(Engine e) {
                    e.ScheduleEvent<UserXCrystalsChangedEvent>(entity);
                });
            }
        }

        void ComponentLifecycle.DetachFromEntity(Entity entity) { }

        [CompilerGenerated]
        sealed class AttachToEntity_003Ec__AnonStorey64 {
            internal Entity entity;

            internal void _003C_003Em__F9(Engine e) => e.ScheduleEvent<UserXCrystalsChangedEvent>(entity);
        }
    }
}