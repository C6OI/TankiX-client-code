using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1503470104769L)]
    public class ElevatedAccessUserBanUserEvent : ElevatedAccessUserBasePunishEvent {
        public string Type { get; set; }
    }
}