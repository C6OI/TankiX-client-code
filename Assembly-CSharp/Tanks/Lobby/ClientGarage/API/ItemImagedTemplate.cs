using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(635925268332192644L)]
    public interface ItemImagedTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        ImageItemComponent imageItem();
    }
}