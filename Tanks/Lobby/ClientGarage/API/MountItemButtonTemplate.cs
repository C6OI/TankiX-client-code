using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1444371881265L)]
    public interface MountItemButtonTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        MountItemButtonTextComponent mountItemButtonText();
    }
}