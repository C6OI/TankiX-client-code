using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1503493668447L)]
    public class ElevatedAccessUserRunCommandEvent : Event {
        public string Command { get; set; }
    }
}