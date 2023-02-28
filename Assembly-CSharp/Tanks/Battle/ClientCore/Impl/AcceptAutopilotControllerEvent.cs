using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1450950143213L)]
    [Shared]
    public class AcceptAutopilotControllerEvent : Event {
        public int Version { get; set; }
    }
}