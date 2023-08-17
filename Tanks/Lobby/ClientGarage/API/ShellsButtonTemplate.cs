using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(635900984027797420L)]
    public interface ShellsButtonTemplate : Template {
        [PersistentConfig]
        [AutoAdded]
        ShellsButtonTextComponent shellsButtonText();
    }
}