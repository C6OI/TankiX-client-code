using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636046246236451827L)]
    public class DefaultSkinComponent : Component {
        public long DefaultSkin { get; set; }
    }
}