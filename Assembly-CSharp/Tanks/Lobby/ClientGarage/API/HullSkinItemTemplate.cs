using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1469607978249L)]
    public interface HullSkinItemTemplate : Template {
        [AutoAdded]
        HullSkinItemComponent hullSkinItem();
    }
}