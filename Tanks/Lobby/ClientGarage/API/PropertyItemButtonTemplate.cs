using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1447071665211L)]
    public interface PropertyItemButtonTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        PropertyItemButtonTextComponent propertyItemButtonText();
    }
}