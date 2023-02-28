using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(635990023494048890L)]
    public class ServerShutdownTextComponent : Component {
        public string Text { get; set; }
    }
}