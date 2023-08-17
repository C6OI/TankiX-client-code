using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class EntityUnshareCommand : EntityCommand {
        [Inject] public static SharedEntityRegistry SharedEntityRegistry { get; set; }

        public override void Execute(Engine engine) => Flow.Current.ScheduleWith(DeleteEntity);

        void DeleteEntity(Engine engine) {
            engine.DeleteEntity(Entity);
            SharedEntityRegistry.SetUnshared(Entity.Id);
        }

        public override string ToString() => string.Format("EntityUnshareCommand: Entity={0}", Entity);
    }
}