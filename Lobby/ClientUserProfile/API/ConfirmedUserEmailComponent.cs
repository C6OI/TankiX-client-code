using System.Runtime.CompilerServices;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientUserProfile.API {
    [SerialVersionUID(1457515023113L)]
    [Shared]
    public class ConfirmedUserEmailComponent : Component, ComponentLifecycle {
        [Inject] public static EngineService EngineService { get; set; }

        public string Email { get; set; }

        public bool Subscribed { get; set; }

        void ComponentLifecycle.AttachToEntity(Entity entity) {
            if (entity != null) {
                EngineService.ExecuteInFlow(delegate(Engine e) {
                    e.ScheduleEvent<ConfirmedUserEmailChangedEvent>(entity);
                });
            }
        }

        void ComponentLifecycle.DetachFromEntity(Entity entity) { }

        [CompilerGenerated]
        sealed class AttachToEntity_003Ec__AnonStorey2F {
            internal Entity entity;

            internal void _003C_003Em__37(Engine e) => e.ScheduleEvent<ConfirmedUserEmailChangedEvent>(entity);
        }
    }
}