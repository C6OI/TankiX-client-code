using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(635985836750478550L)]
    public class BattleShutdownTextComponent : Component {
        public string Text { get; set; }
    }
}