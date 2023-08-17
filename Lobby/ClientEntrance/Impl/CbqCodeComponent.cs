using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.Impl {
    [SerialVersionUID(636068760971706109L)]
    [Shared]
    public class CbqCodeComponent : Component {
        public string Mail { get; set; }

        public string Code { get; set; }
    }
}