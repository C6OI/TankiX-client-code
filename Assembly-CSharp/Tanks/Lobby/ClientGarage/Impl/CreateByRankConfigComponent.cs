using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [SerialVersionUID(1495091046209L)]
    public class CreateByRankConfigComponent : Component {
        public int[] UserRankListToCreateItem { get; set; }
    }
}