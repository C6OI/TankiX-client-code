using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636397848481315222L)]
    public interface CardTierTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        CardTierComponent cardTier();
    }
}