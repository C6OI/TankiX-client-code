using System.Runtime.CompilerServices;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.API {
    [Shared]
    [SerialVersionUID(9171752353079252620L)]
    public class UserMoneyComponent : Component, ComponentLifecycle {
        [Inject] public static EngineService EngineService { get; set; }

        public long Money { get; set; }

        void ComponentLifecycle.AttachToEntity(Entity entity) {
            if (entity != null) {
                EngineService.ExecuteInFlow(delegate(Engine e) {
                    e.ScheduleEvent<UserMoneyChangedEvent>(entity);
                });
            }
        }

        void ComponentLifecycle.DetachFromEntity(Entity entity) { }

        [CompilerGenerated]
        sealed class AttachToEntity_003Ec__AnonStorey24 {
            internal Entity entity;

            internal void _003C_003Em__1C(Engine e) => e.ScheduleEvent<UserMoneyChangedEvent>(entity);
        }
    }
}