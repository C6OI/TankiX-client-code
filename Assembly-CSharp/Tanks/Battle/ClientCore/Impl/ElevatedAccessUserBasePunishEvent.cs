using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class ElevatedAccessUserBasePunishEvent : Event {
        public string Uid { get; set; }

        public string Reason { get; set; }
    }
}